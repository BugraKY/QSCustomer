let focuser = setInterval(() => window.dispatchEvent(new Event('focus')), 500);
let font = ""

function printreport() {
    printJS({
        printable: 'printSec',
        type: 'html',
        style:`@page { size: A4 landscape;size: 287mm 210mm;margin:6px; }`,
        onPrintDialogClose: () => {
            clearInterval(focuser);
            window.close();
        },
        onError: () => {
            clearInterval(focuser);
            window.close();
            // do your thing..
        }
    });
};