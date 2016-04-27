(function () {
    'use strict';

    angular.module('Menu')
        .controller('MenuCtrl', function ($scope, $state, AccountDataSvc) {
            //console.log($scope.userContext);
            var self = this;

            self.successLogout = function () {
                $scope.userContext.userName = '';
                $scope.userContext.role = '';
                $state.go('home');
            };

            $scope.logout = function () {
                AccountDataSvc.logout().then(self.successLogout);
            };
        })
        .directive('menu', function () {
            return {
                scope: true,
                restrict: 'E',
                templateUrl: 'js/angular/app/MainMenu/templates/menu.html',
                controller: 'MenuCtrl'
            };
        })
})();