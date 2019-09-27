(function () {
    'use strict';

    var app = angular.module('app');

    var serviceId = 'cookiesManager';
    app.factory(serviceId, function ($cookieStore) {
        var cookiesManagerDataFactory = {};

        var _putInCoockieStoreByKey = function (key, value) {
            $cookieStore.put(key, value);
        };

        var _putInCoockieStoreByKeyArray = function (array) {
            array.forEach(function (cookie) {
                $cookieStore.put(cookie.key, cookie.value);
            });
        };

        var _removeFromCookieStoreByKey = function (key) {
            $cookieStore.remove(key);
        };

        var _removeFromCookieStoreByKeyArray = function (array) {
            array.forEach(function(key) {
                $cookieStore.remove(key);
            });
        };

        var _getFromCookieStoreByKey = function (key) {
            return $cookieStore.get(key);
        };

        cookiesManagerDataFactory.putInCoockieStoreByKeyArray = _putInCoockieStoreByKeyArray;
        cookiesManagerDataFactory.putInCoockieStoreByKey = _putInCoockieStoreByKey;
        cookiesManagerDataFactory.removeFromCookieStoreByKey = _removeFromCookieStoreByKey;
        cookiesManagerDataFactory.removeFromCookieStoreByKeyArray = _removeFromCookieStoreByKeyArray;
        cookiesManagerDataFactory.getFromCookieStoreByKey = _getFromCookieStoreByKey;

        return cookiesManagerDataFactory;
    });
})();