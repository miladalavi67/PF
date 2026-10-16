function persianToEnglishNumber(persianNumber: string) {
    const persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    const englishDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    let englishNumber = persianNumber;
    for (let i = 0; i < 10; i++) {
        const regex = new RegExp(persianDigits[i], 'g');
        englishNumber = englishNumber.replace(regex, englishDigits[i]);
    }
    return englishNumber;
}



export {
    persianToEnglishNumber
}