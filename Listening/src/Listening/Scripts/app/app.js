(function () {
    'use strict';

    angular.module('Account', ['ngResource', 'ui.router']);
    angular.module('Menu', ['Account', 'ui.router']);
    angular.module('AdditionFunctionality', []);
    angular.module('Common', ['ngFileUpload']);
    angular.module('Administration', ['ui.router', 'ngFileUpload', 'Common', 'AdditionFunctionality']);
    angular.module('TextForListening', ['ui.router', 'ngResource', 'ui.bootstrap', 'ngAnimate', 'focus-if', 'Common',
                                        'AdditionFunctionality', 'cgBusy', 'ngMaterial']);


    angular.module('app', ['ngRoute', 'ui.router', /*'templateCache', */'Account', 'Menu', 'Common', 'Administration', 'TextForListening'
        /*, 'AdditionFunctionality'*/])
        .config(function ($routeProvider, /*$stateProvider,*/ $locationProvider, $stateProvider, $urlRouterProvider) {

            var pathRoot = 'js/angular/app/templates/';
            var pathAccount = 'js/angular/app/Account/templates/';
            var pathAdmin = 'js/angular/app/Administration/templates/';
            var pathTextForListening = 'js/angular/app/TextForListening/templates/';

            $stateProvider
                .state('home', {
                    url: '/home',
                    templateUrl: pathRoot + 'about.html'
                })
                .state('about', {
                    url: '/about',
                    templateUrl: pathRoot + 'about.html'
                })
                .state('contacts', {
                    url: '/contacts',
                    templateUrl: pathRoot + 'contacts.html'
                })
                .state('login', {
                    url: '/login',
                    controller: "LoginCtrl",
                    templateUrl: pathAccount + 'login.html'
                })
                .state('register', {
                    url: '/register',
                    controller: "RegisterCtrl",
                    templateUrl: pathAccount + 'register.html'
                })
                //.state("allTextsDescription", {
                //    url: '/allTextsDescription',
                //    templateUrl: pathTextForListening + 'allTexts.html',
                //    controller: 'AllTextsCtrl'
                //})
                .state("allTextsDescription", {
                    url: '/allTextsDescription',
                    templateUrl: pathTextForListening + 'allTextsForGuessing.html',
                    controller: 'AllTextsForGuessingCtrl'
                })
                .state("currentText", {
                    url: '/text/:textId/:title/:subTitle/:audio',
                    templateUrl: pathTextForListening + 'text.html',
                    controller: 'TextCtrl'
                })
                .state('administration', {
                    url: '/administration',
                    templateUrl: pathAdmin + 'allTextsForEditing.html',
                    controller: 'AllTextsForEditingCtrl'
                })
                .state('administrationEdit', {
                    url: '/administrationEdit/:textId',
                    templateUrl: pathAdmin + 'textEdit.html',
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