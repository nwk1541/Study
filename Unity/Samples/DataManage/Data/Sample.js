const xlsx = require('xlsx');
const workbook = xlsx.readFile('./Data_Game.xlsx'); // 현재 읽을 엑셀

const json = {};
let i = workbook.SheetNames.length;

while(i--) {
    const sheetname = workbook.SheetNames[i];
    json[sheetname] = xlsx.utils.sheet_to_json(workbook.Sheets[sheetname]);

    // 엑셀의 각 시트당 하나씩 .json 파일 생성
    const fs = require('fs');
    fs.writeFile(sheetname + '.json', JSON.stringify(json[sheetname], null, 2), (err) => {
        if(err) throw err;
        console.log(sheetname + "Sheet Convert Clear");
    });
}