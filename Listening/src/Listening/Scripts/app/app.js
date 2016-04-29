(function () {
    'use strict';
    
    angular.module('Account', ['ngResource', 'ui.router']);
    angular.module('Menu', ['Account', 'ui.router']);
    angular.module('AdditionFunctionality', []);
    angular.module('Common', ['ngFileUpload']);
    angular.module('Administration', ['ui.router', 'ngFileUpload', 'Common', 'AdditionFunctionality']);
    angular.module('TextForListening', ['ui.router', 'ngResource', 'ui.bootstrap', 'ngAnimate', 'focus-if', 'Common',
                                        'AdditionFunctionality', 'cgBusy', 'ngMaterial']);

    angular.module('app', ['ngRoute', 'ui.router', 'Account', 'Menu', 'Common', 'Administration', 'TextForListening', 'templates'
        /*, 'AdditionFunctionality'*/])
        .config(function ($routeProvider, /*$stateProvider,*/ $locationProvider, $stateProvider, $urlRouterProvider) {

            //var pathRoot = 'js/angular/app/templates/';
            var pathAccount = 'Account/';
            var pathAdmin = 'Administration/';
            var pathTextForListening = 'TextForListening/';

            var provide = function (path) {
                return function ($templateCache) {
                    return $templateCache.get(path);
                }
            };

            $stateProvider
                .state('home', {
                    url: '/home',
                    templateProvider: provide('/home.html')
                })
                .state('about', {
                    url: '/about',
                    templateProvider: provide('/about.html')
                })
                .state('contacts', {
                    url: '/contacts',
                    templateProvider: provide('/contacts.html')
                })
                .state('login', {
                    url: '/login',
                    controller: "LoginCtrl",
                    templateProvider: provide(pathAccount + 'login.html')
                })
                .state('register', {
                    url: '/register',
                    controller: "RegisterCtrl",
                    templateProvider: provide(pathAccount + 'register.html')

                })
                //.state("allTextsDescription", {
                //    url: '/allTextsDescription',
                //    templateUrl: pathTextForListening + 'allTexts.html',
                //    controller: 'AllTextsCtrl'
                //})
                .state("allTextsDescription", {
                    url: '/allTextsDescription',
                    templateProvider: provide(pathTextForListening + 'allTextsForGuessing.html'),
                    controller: 'AllTextsForGuessingCtrl'
                })
                .state("currentText", {
                    url: '/text/:textId/:title/:subTitle/:audio',
                    templateProvider: provide(pathTextForListening + 'text.html'),
                    controller: 'TextCtrl'
                })
                .state('administration', {
                    url: '/administration',
                    templateProvider: provide(pathAdmin + 'allTextsForEditing.html'),
                    controller: 'AllTextsForEditingCtrl'
                })
                .state('administrationEdit', {
                    url: '/administrationEdit/:textId',
                    templateProvider: provide(pathAdmin + 'textEdit.html'),
                    controller: 'TextEditCtrl'
                });
            //.state("currentTextEdit", {
            //    url: '/text/:textId/:title/:subTitle/:audio',
            //    templateUrl: pathTextForListening + 'text.html',
            //    controller: 'TextCtrl'
            //});

            $urlRouterProvider.otherwise('/allTextsDescription');
        })
        .run(function ($rootScope) {
            $rootScope.userContext = userContext;
        });
    
})();