import * as fs from 'node:fs';
import * as readline from 'node:readline';
import anyAscii from 'any-ascii';

interface VocabEntry {
    term: string;
    definitions?: string[];
}

const MIN_FREQUENCY = 1_200_000;
const DATA = './data';
const OUTPUT = './out';

const DEFINITION_FILTERS: RegExp[] = [
    /name/,
    /stands for/, /is an acronym/,
    /is a town/, /is a city/, /is a country/, /is a state/, /capital of/,
    /is the [A-Z][a-z]+ word for/,
    /misspelling/,
];

const createReadlineInterface = (dataName: string) => {
    return readline.createInterface({
        input: fs.createReadStream(`${DATA}/${dataName}`),
        crlfDelay: Infinity,
    });
};

const main = async (): Promise<void> => {
    const vocabFile = createReadlineInterface('vocab.jsonl');

    const wordDefinitions: Record<string, string> = {};

    for await (const json of vocabFile) {
        if (!json.trim()) continue;

        const { term, definitions } = JSON.parse(json) as VocabEntry;

        if (!definitions || definitions.length === 0) {
            // skip, not defined
            continue;
        }

        const normalizedTerm = anyAscii(term.toLowerCase());

        if (normalizedTerm.length < 5 || normalizedTerm.length > 6) {
            // skip, wrong size
            continue;
        }

        if (!/^[a-zA-Z]+$/.test(normalizedTerm)) {
            // skip, not just ascii
            continue;
        }

        const filteredDefinitions: string[] = [];
        for (const definition of definitions.map((d) => anyAscii(d).replaceAll('"', "'"))) {
            if (DEFINITION_FILTERS.some((filter) => filter.test(definition))) {
                // skip, triggered some filters
                continue;
            }

            filteredDefinitions.push(definition);
        }

        if (filteredDefinitions.length == 0) {
            // skip, no correct definitions
            continue;
        }

        wordDefinitions[normalizedTerm] = filteredDefinitions[0];
    }

    console.log(`Total words defined: ${Object.keys(wordDefinitions).length}`);

    const wordFrequencies: Record<string, number> = {};
    let isHeader = true;

    const freqFile = createReadlineInterface('unigram_freq.csv');

    for await (const line of freqFile) {
        if (isHeader) {
            isHeader = false;
            continue;
        }

        const [rawWord, rawCount] = line.split(',');
        if (!rawWord || !rawCount) continue;

        const word = rawWord.trim();
        const count = Number(rawCount);

        if (wordDefinitions[word] === undefined) {
            // skip, not defined
            continue;
        }

        if (count < MIN_FREQUENCY) {
            // skip, not used much
            continue;
        }

        wordFrequencies[word] = count;
    }

    console.log(`Total frequent words matched: ${Object.keys(wordFrequencies).length}`);

    const wordsStream = fs.createWriteStream(`${OUTPUT}/words.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });

    const wordsCodeStream = fs.createWriteStream(`${OUTPUT}/words-code.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });

    for (const word of Object.keys(wordFrequencies)) {
        const definition = wordDefinitions[word];
        const frequency = wordFrequencies[word];

        wordsStream.write(`${word};${definition};${frequency}\n`);
        wordsCodeStream.write(`{"${word}", "${definition}" },\n`);
    }

    wordsStream.end();
    wordsCodeStream.end();
};

main();