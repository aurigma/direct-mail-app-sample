const fs = require('fs-extra');
const path = require('path');

const targetDir = "publish";
const packageJson = require('../package.json');

const ignoreItems = ["package.json", "README.md", "LICENSE.md", "dist"].map(x => x.toLowerCase());

fs.removeSync(targetDir);
fs.mkdirSync(targetDir);

const items = fs.readdirSync("./");

items.forEach(item => {
    if (ignoreItems.indexOf(item.toLowerCase()) !== -1) {
        fs.copySync(item, targetDir + path.sep + item)
    }
})

packageJson.scripts = {};

fs.writeFileSync(targetDir + path.sep + "package.json", JSON.stringify(packageJson, null, 4), "utf-8")
