describe("RegisterCtrlTest", function () {

    var factory;

    var errorEmailMessage = 'Incorrect email',
        digitPasswordErrorMessage = 'Password should contain at least one digit',
        bigLetterPasswordErrorMessage = 'Password should contain at least one big letter',
        littleLetterPasswordErrorMessage = 'Password should contain at least one little letter',
        specialCharacterPasswordErrorMessage = 'Password should contain at least one special character',
        passwordConfirmError = 'Passwords are not the same';

    beforeEach(function () {
        module('Account');

        inject(function ($injector) {
            factory = $injector.get('CheckDataSvc');
        });
    });

    describe('Email Check', function () {
        it("Should return 'Incorrect email' string when input string contain only letters", function () {
            var result = factory.emailCheck('asd');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the end", function () {
            var result = factory.emailCheck('asd@');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the middle (without dots)", function () {
            var result = factory.emailCheck('asd@asd');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the middle and dot in the end", function () {
            var result = factory.emailCheck('asd@asd.');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the middle and dot in the end (2)", function () {
            var result = factory.emailCheck('asd@asd.asd.');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the middle and dot in the end (3)", function () {
            var result = factory.emailCheck('asd@asd.asd.sdfsdf.');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should return 'Incorrect email' string when input string contain letters and '@' symbol in the middle and one letter after dot", function () {
            var result = factory.emailCheck('asd@asd.asd.sdfsdf.a');
            expect(result).toBe(errorEmailMessage);
        });

        it("Should not return 'Incorrect email' string correct address entered", function () {
            var result = factory.emailCheck('asd@asd.dfsdf');
            expect(result).not.toBe(errorEmailMessage);
        });

        it("Should not return 'Incorrect email' string correct address entered (multiple domains)", function () {
            var result = factory.emailCheck('asd@asd.asd.sdfsdf.sdf.sdhg.fgh.fg.hsd');
            expect(result).not.toBe(errorEmailMessage);
        });

        it("Should not return 'Incorrect email' string correct address entered (digits in email)", function () {
            var result = factory.emailCheck('a456321sd@asd.asd.sdfsdf.sdf.sdhg.fgh.fg.hsd');
            expect(result).not.toBe(errorEmailMessage);
        });
    });

    describe('Password Check', function () {

        it('Should return error message for digits missing', function () {
            var result = factory.passwordCheck('asdasddsaA@');
            expect(result).toBe(digitPasswordErrorMessage);
        });

        it('Should return error message for special character missing', function () {
            var result = factory.passwordCheck('asdasddsaA777');
            expect(result).toBe(specialCharacterPasswordErrorMessage);
        });

        it('Should return error message for big character missing', function () {
            var result = factory.passwordCheck('asdasddsa%7');
            expect(result).toBe(bigLetterPasswordErrorMessage);
        });

        it('Should return error message for little character missing', function () {
            var result = factory.passwordCheck('TYAUSGDJHGJ%7');
            expect(result).toBe(littleLetterPasswordErrorMessage);
        });

        it('Should not return error any error message', function () {
            var result = factory.passwordCheck('HJKDHF%^&wefhjk678');
            expect(result).not.toBe(littleLetterPasswordErrorMessage);
            expect(result).not.toBe(bigLetterPasswordErrorMessage);
            expect(result).not.toBe(specialCharacterPasswordErrorMessage);
            expect(result).not.toBe(digitPasswordErrorMessage);
        });

        it('Should not return error any error message', function () {
            var result = factory.passwordCheck('dfjkbl&*(rhjksdfy839F');
            expect(result).not.toBe(littleLetterPasswordErrorMessage);
            expect(result).not.toBe(bigLetterPasswordErrorMessage);
            expect(result).not.toBe(specialCharacterPasswordErrorMessage);
            expect(result).not.toBe(digitPasswordErrorMessage);
        });

        it('Should not return error any error message', function () {
            var result = factory.passwordCheck('bi478359kJHdfs&32');
            expect(result).not.toBe(littleLetterPasswordErrorMessage);
            expect(result).not.toBe(bigLetterPasswordErrorMessage);
            expect(result).not.toBe(specialCharacterPasswordErrorMessage);
            expect(result).not.toBe(digitPasswordErrorMessage);
        });
    });

    describe('Password confirmation check', function () {
        it('Should return error message for passwords unequality', function () {
            var result = factory.confirmPasswordCheck('bi478359kJHdfs&32', 'asdasdfdgdfg');
            expect(result).toBe(passwordConfirmError);
        });

        it('Should return error message for passwords unequality', function () {
            var result = factory.confirmPasswordCheck('asd', 'asdasdfdgdfg');
            expect(result).toBe(passwordConfirmError);
        });

        it('Should return error message for passwords unequality', function () {
            var result = factory.confirmPasswordCheck('asdasdfdgdfgas', 'asdasdfdgdfg');
            expect(result).toBe(passwordConfirmError);
        });

        it('Should not return error message for passwords unequality', function () {
            var result = factory.confirmPasswordCheck('asdasdfdgdfg', 'asdasdfdgdfg');
            expect(result).not.toBe(passwordConfirmError);
        });
    });
});