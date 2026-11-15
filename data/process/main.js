// const fs = require('fs');
// const readline = require('readline');
// const anyAsciiModule = require('any-ascii');
// const anyAscii =
//     anyAsciiModule.default || anyAsciiModule.anyAscii || anyAsciiModule;
import fs from 'node:fs';
const MIN_FREQUENCY = 300000;
const DATA = './data';
const OUTPUT = './out';
const main = async () => {
    const vocabFile = readline.createInterface({
        input: fs.createReadStream(`${DATA}/vocab.jsonl`),
        crlfDelay: Infinity,
    });
    const wordDefinitions = {};
    for await (const json of vocabFile) {
        if (!json.trim())
            continue;
        const { term, definitions } = JSON.parse(json);
        if (!definitions || definitions.length === 0) {
            // skip, not defined
            continue;
        }
        if (/[A-Z]/.test(term[0])) {
            // skip, uppercase names
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
        wordDefinitions[normalizedTerm] = definitions.map((def) => anyAscii(def).replaceAll('"', "'"));
    }
    console.log(`Total words defined: ${Object.keys(wordDefinitions).length}`);
    const frequentWords = new Set();
    let isHeader = true;
    const freqFile = readline.createInterface({
        input: fs.createReadStream(`${DATA}/unigram_freq.csv`),
        crlfDelay: Infinity,
    });
    for await (const line of freqFile) {
        if (!line.trim())
            continue;
        if (isHeader) {
            isHeader = false;
            continue;
        }
        const [rawWord, rawCount] = line.split(',');
        const word = rawWord.trim();
        const count = Number(rawCount);
        if (wordDefinitions[word] && count >= MIN_FREQUENCY) {
            frequentWords.add(word);
        }
    }
    console.log(`Total frequent words matched: ${frequentWords.size}`);
    const wordsStream = fs.createWriteStream(`${OUTPUT}/words.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });
    const wordsCodeStream = fs.createWriteStream(`${OUTPUT}/words-code.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });
    const frequentWordsStream = fs.createWriteStream(`${OUTPUT}/frequent-words.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });
    const frequentWordsCodeStream = fs.createWriteStream(`${OUTPUT}/frequent-words-code.txt`, {
        flags: 'w',
        encoding: 'utf8',
    });
    for (const word of Object.keys(wordDefinitions)) {
        const definition = wordDefinitions[word][0];
        wordsStream.write(`${word};${definition}\n`);
        wordsCodeStream.write(`{"${word}", "${definition}" },\n`);
        if (frequentWords.has(word)) {
            frequentWordsStream.write(`${word};${definition}\n`);
            frequentWordsCodeStream.write(`{"${word}", "${definition}" },\n`);
        }
    }
    wordsStream.end();
    wordsCodeStream.end();
    frequentWordsStream.end();
    frequentWordsCodeStream.end();
};
main();
