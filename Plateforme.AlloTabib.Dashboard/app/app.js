'use strict';

var app = angular
  .module('app', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngRoute',
    'ngClipboard',
    'ngSanitize',
    'ngTouch',
    'ngClipboard',
    'mgcrea.ngStrap',
    'ui.bootstrap',
    'mgcrea.ngStrap.popover'

  ]).directive('onFinishRender', function ($timeout) {
      return {
          restrict: 'A',
          link: function (scope, element, attr) {
              if (scope.$last === true) {
                  $timeout(function () {
                      scope.$emit('ngRepeatFinished');
                  });
              }
          }
      };
  });

app.config(function ($routeProvider, $httpProvider) {
    $httpProvider.defaults.headers.post = {
        'Content-Type': 'application/json;charset=utf-8'
    };
    $httpProvider.defaults.headers.patch = {
        'Content-Type': 'application/json;charset=utf-8'
    };
    $httpProvider.defaults.headers.delete = {
        'Content-Type': 'application/json;charset=utf-8'
    };
    $routeProvider
         .when('/', {
             templateUrl: '/app/views/home/login.html',
             controller: 'loginController'
         })
        .when('/calendrier', {
            templateUrl: '/app/views/calendrier/calendrier.html',
            controller: 'calendrierController'
            
        })

        .when('/mespatients', {
            templateUrl: '/app/views/mespatients/mespatients.html',
            controller: 'mespatientsController'
        })
       
         .when('/moncompte', {
             templateUrl: '/app/views/moncompte/moncompte.html',
             controller: 'moncompteController'
         })

        .otherwise({
            templateUrl: '/app/views/calendrier/calendrier.html',
            controller: 'calendrierController'
        });
});

//app.run(function ($rootScope, $location) {
        // register listener to watch route changes
//        $rootScope.$on( "$routeChangeStart", function(event, next, current) {
//            if ( $rootScope.loggedUser == null ) {
//                // no logged user, we should be going to #login
//                if ( next.templateUrl == "/app/views/home/login.html" ) {
//                    // already going to #login, no redirect needed
//                } else {
//                    // not going to #login, we should redirect now
//                    $location.path( "/" );
//                }
//            }         
//        });
//});


app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

app.directive('validPasswordC', function () {
    return {
        require: 'ngModel',
        link: function(scope, elm, attrs, ctrl) {

            ctrl.$setValidity('noMatch', true);

            attrs.$observe('validPasswordC', function(newVal) {
                if (newVal === 'true') {
                    ctrl.$setValidity('noMatch', true);
                } else {
                    ctrl.$setValidity('noMatch', false);
                }
            });
        }
    };
});


// App Settings
var apiUri = 'http://allotabib-api.allotabib.net/';


app.constant('appSettings', {
    praticienUri: apiUri + "praticien/", //avoir tous les praticiens
    patientUri: apiUri + "patient/",
    contactUri: apiUri + "contact/",
    alloTabibUserApiUri: apiUri + 'account/ispraticien',
    userIsAuth: apiUri + 'account/ispraticien',
    getPraticienByEmail: apiUri + 'praticien/getpraticien',
    calendrierUri: apiUri + 'calendriers/calendrierpra',
    rendezvousUri: apiUri + 'rendezvous/getpatients',
    jourFerieURi: apiUri + 'jourferie',
    rendezvous: apiUri + 'rendezvous/',
    creneauUri: apiUri + 'creneau'
});

app.constant('defaultPageSize', {
    mainMaxPageSize: 50
});



