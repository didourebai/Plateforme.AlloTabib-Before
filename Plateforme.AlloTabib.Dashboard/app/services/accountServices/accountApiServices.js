'use strict';
//Add factory 
app.factory('authService', function ($http, appSettings) {

    var serviceBase = appSettings.userIsAuth;
    //declare factory object for this service
    var authServiceFactory = {};

    // Add new praticien.
    var _isUserAuthenticated = function (loginData) {

        return $http({
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            url: serviceBase,
            data: loginData
        }).then(function (results) {
            return results;
        });
    };
    /****************************************** Public Methods. ******************************************
  /*****************************************************************************************************/


    authServiceFactory.isUserAuthenticated = _isUserAuthenticated;

    // Return.
    return authServiceFactory;
});

//app.factory('authService', function (appSettings, $http) {

//    var authServiceFactory = {};
//    var serviceBase = appSettings.alloTabibUserApiUri;


//    var _authentication = {
//        isAuth: false,
//        userName: ""
//    };

//    var _isUserAuthenticated = function (loginData) {
//        return $http({
//                    method: 'POST',
//                    url: serviceBase,
//                    data: loginData,
//                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
//        }).then(function (results) {
//            if (results.data == "true") {
//                _authentication.isAuth = true;
//                _authentication.userName = loginData.userName;
//            }
//                    //return results;
//          });
//    };

//    var _login = function(loginData) {

//        $http.post(serviceBase, loginData, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function () {
//            _authentication.isAuth = true;
//            _authentication.userName = loginData.userName;
//        }).error(function (err, status) {
          
//        });
//    };

//    var _logOut = function () {
//        _authentication.isAuth = false;
//        _authentication.userName = "";
//    };

//    authServiceFactory.isUserAuthenticated = _isUserAuthenticated;
//    authServiceFactory.logOut = _logOut;
//    authServiceFactory.login = _login;

//    return authServiceFactory;
//});