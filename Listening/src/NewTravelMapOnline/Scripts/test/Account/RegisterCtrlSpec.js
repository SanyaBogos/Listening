describe("RegisterCtrlTest", function () {
    var $rootScope,
        $scope,
        $controller,
        AccountDataSvcMock,
        CheckDataSvcMock;

    var promiseRegistry;

    beforeEach(module('Account'));

    beforeEach(inject(function ($q) {
        promiseRegistry = $q.defer().promise;
    }));

    beforeEach(inject(function ($controller, $rootScope) {
        CheckDataSvcMock = jasmine.createSpyObj('CheckDataSvc', ['emailCheck', 'passwordCheck', 'confirmPasswordCheck']);
        AccountDataSvcMock = jasmine.createSpy('AccountDataSvc');
        AccountDataSvcMock.registry = jasmine.createSpy('registry').and.returnValue(promiseRegistry);

        $scope = $rootScope.$new();

        $controller = $controller('RegisterCtrl', {
            $scope: $scope,
            AccountDataSvc: AccountDataSvcMock,
            CheckDataSvc: CheckDataSvcMock
        });
    }));

    it('Should be called function registry of AccountDataSvc', function () {
        $scope.user = {};

        $scope.registryClick();

        expect(AccountDataSvcMock.registry).toHaveBeenCalled();
    });

    it('Should be called function emailCheck of CheckDataSvc', function () {
        $scope.user = {};

        $scope.emailCheck();

        expect(CheckDataSvcMock.emailCheck).toHaveBeenCalled();
    });

    it('Should be called function passwordCheck of CheckDataSvc', function () {
        $scope.user = {};

        $scope.passwordCheck();

        expect(CheckDataSvcMock.passwordCheck).toHaveBeenCalled();
    });

    it('Should be called function confirmPasswordCheck of CheckDataSvc', function () {
        $scope.user = {};

        $scope.confirmPasswordCheck();

        expect(CheckDataSvcMock.confirmPasswordCheck).toHaveBeenCalled();
    });
});