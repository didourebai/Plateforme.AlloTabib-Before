(function () {
    'use strict';

    var app = angular.module('app');

    var serviceId = 'userManager';
    app.factory(serviceId, function (cookiesManager) {
        var userManagerDataFactory = {};

        var _userProfile = {
            isLogged: false,
            login: null,
            password: null,
            isAdmin: null
        };

        var _setUserLogged = function (isLogged) {
            _userProfile.isLogged = isLogged;
        };

        var _logout = function () {
            _userProfile = {
                isLogged: false,
                login: null,
                password: null,
                isAdmin: null,
            };

            cookiesManager.removeFromCookieStoreByKeyArray([
                    'koda_user_login',
                    'koda_user_password',
                    'koda_user_is_admin',
                    'koda_user_logged',
                    'access_token',
                    'refresh_token',
                    'revoke_token',
                    'token_expires_in',
                    'ksso_user_name',
                    'ksso_user_id',
                    'koda_user_ksso_id'
            ]);
        };

        var _setUserLogin = function (login) {
            _userProfile.login = login;
        };

        var _setUserPassword = function (pw) {
            _userProfile.password = pw;
        };

        var _getUserLogin = function () {
            //return _userProfile.login;
            return cookiesManager.getFromCookieStoreByKey('koda_user_login');
        };

        var _getKssoUsername = function () {
            return cookiesManager.getFromCookieStoreByKey('ksso_user_name');
        };

        var _getKssoUserId = function () {
            return cookiesManager.getFromCookieStoreByKey('ksso_user_id');
        };

        var _isLogged = function () {
            //return _userProfile.isLogged;
            return cookiesManager.getFromCookieStoreByKey('koda_user_logged');
        };

        var _getUserPassword = function () {
            //return _userProfile.password;
            return cookiesManager.getFromCookieStoreByKey('koda_user_password');
        };

        var _setUserArea = function (isAdmin) {
            _userProfile.isAdmin = isAdmin;
        };

        var _getUserArea = function () {
            return _userProfile.isAdmin;
        };

        userManagerDataFactory.setUserLogin = _setUserLogin;
        userManagerDataFactory.setUserPassword = _setUserPassword;
        userManagerDataFactory.getUserLogin = _getUserLogin;
        userManagerDataFactory.getUserPassword = _getUserPassword;
        userManagerDataFactory.isLogged = _isLogged;
        userManagerDataFactory.setUserLogged = _setUserLogged;
        userManagerDataFactory.logout = _logout;
        userManagerDataFactory.setUserArea = _setUserArea;
        userManagerDataFactory.getUserArea = _getUserArea;
        userManagerDataFactory.getKssoUsername = _getKssoUsername;
        userManagerDataFactory.getKssoUserId = _getKssoUserId;

        return userManagerDataFactory;
    });
})();