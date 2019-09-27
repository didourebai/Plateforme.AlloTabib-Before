'use strict';

var app = angular
  .module('app', [
    'ngAnimate',
    'ngCookies',
    'oc.lazyLoad',
    'ngResource',
    'ngRoute',
    'ngClipboard',
    'ngSanitize',
    'ngTouch',
    'ngClipboard',
    'mgcrea.ngStrap',
    'ui.bootstrap',
    'mgcrea.ngStrap.popover',
      'ngTouch',
      'ui.router'

  ]);

//route config
app.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider
        // route to show our basic form (/form)
        .state('home', {
            url: '/home',
            templateUrl: '/app/views/home/home.html',
            controller: 'homeController',
            resolve: {
                // Toutes les propriétés de “resolve” doivent retourner une promise
                // et sont exécutuées avant que la vue ne soit chargée
                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                    // you can lazy load files for an existing module
                    return $ocLazyLoad.load('home');
                }]
            }

        })
        //**************************************************************************
        // partie de wizard assisté pour prendre un rendez vous*******************
           .state('rendez-vous', {
               url: '/rendezvous/confirmation',
               templateUrl: '/app/views/rendezvous/rendezvous.html',
               /*controller: 'rendezVousController'*/
           })

          .state('rendez-vous.identification', {
              url: '/identification',
              templateUrl: '/app/views/rendezvous/01_identification.html',
              data: { actualFormIndex: 1 }
          })
            .state('rendez-vous.verification', {
                url: '/verification',
                templateUrl: '/app/views/rendezvous/02_verification.html',
                data: { actualFormIndex: 2 }
            })
          .state('rendez-vous.confirmation', {
              url: '/confirmation',
              templateUrl: '/app/views/rendezvous/03_confirmation.html',
              data: { actualFormIndex: 4 }
          })

        //**************************************************************************
        .state('inscriptions', {
            url: '/inscriptions',
            templateUrl: '/app/views/home/inscriptions.html',
            controller: 'homeController',
            resolve: {
                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load('inscriptions');
                }]
            }
        })

    
     .state('inscription', {
         url: '/inscription',
         templateUrl: '/app/views/inscription/inscription.html',
         controller: 'inscriptionController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('inscription');
             }]
         }
     })

     .state('mot-de-passe-oublie', {
         url: '/mot-de-passe-oublie', 
         templateUrl: '/app/views/inscription/motPasseOublie.html',
         controller: 'inscriptionController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('mot-de-passe-oublie');
             }]
         }
     })
     .state('service', {
         url:'/service', 
         templateUrl: '/app/views/login/loginView.html',
         controller: 'inscriptionController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('service');
             }]
         }
     })
     .state('praticien', {
         url: '/praticien',
         templateUrl: '/app/views/praticien/praticien.html',
         controller: 'praticienController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('praticien');
             }]
         }
     })
     .state('contact', {
         url: '/contact', 
         templateUrl: '/app/views/contact/contact.html',
         controller: 'contactController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('contact');
             }]
         }
     })
     .state('inscrirepraticien', {
         url: '/inscrirepraticien', 
         templateUrl: '/app/views/praticien/inscpriptionPraticien.html',
         controller: 'inscriptionPraticienController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('inscrirepraticien');
             }]
         }
     })
     .state('mesrendezvous', {
         url: '/mesrendezvous', 
         templateUrl: '/app/views/inscription/suivreMesRdv.html',
         controller: 'suivreMesRdvController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('mesrendezvous');
             }]
         }
     })
     .state('praticiens', {
         url: '/praticiens', 
         templateUrl: '/app/views/praticien/praticiens.html',
         controller: 'praticienController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('praticiens');
             }]
         }
     })
     .state('rendezvous', {
         url: '/rendezvous', 
         templateUrl: '/app/views/inscription/rendezvous.html',
         controller: 'rendezVousController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('rendezvous');
             }]
         }
     })
     .state('confirmrendezvous', {
         url: '/confirmrendezvous', 
         templateUrl: '/app/views/inscription/priserendezvous.html',
         controller: 'priseRendezVousController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('confirmrendezvous');
             }]
         }
     })
     .state('loginpatient', {
         url: '/loginpatient', 
         templateUrl: '/app/views/login/loginpatient.html',
         controller: 'inscriptionController',
         resolve: {
             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                 return $ocLazyLoad.load('loginpatient');
             }]
         }
     })
      .state('mentionsLegales', {
          url: '/mentionsLegales', 
          templateUrl: '/app/views/contact/conditionsGenerales.html',
          resolve: {
              loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                  return $ocLazyLoad.load('mentionsLegales');
              }]
          }
      })
      .state('/medecins/:nomPrenom', {
          url: '/medecins/:nomPrenom', 
          templateUrl: '/app/views/medecins/pagemedecin.html',
          resolve: {
              loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                  return $ocLazyLoad.load('/medecins/:nomPrenom');
              }]
          }
      })
      .state('monCompte', {
          url: '/monCompte', 
          templateUrl: '/app/views/inscription/monCompte.html',
          controller: 'monCompteController',
          resolve: {
              loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                  return $ocLazyLoad.load('monCompte');
              }]
          }
      })
      .state('blog', {
          url: '/blog', 
          templateUrl: '/app/views/contact/blog.html',
          controller: 'blogController',
          resolve: {
              loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
                  return $ocLazyLoad.load('blog');
              }]
          }
      });

    $urlRouterProvider.otherwise('/home'); 
});

//app.config(function ($routeProvider, $httpProvider) {
//    $httpProvider.defaults.headers.post = {
//        'Content-Type': 'application/json;charset=utf-8'
//    };
//    $httpProvider.defaults.headers.patch = {
//        'Content-Type': 'application/json;charset=utf-8'
//    };
//    $httpProvider.defaults.headers.delete = {
//        'Content-Type': 'application/json;charset=utf-8'
//    };
//    $routeProvider
//        .when('/home', {
//            templateUrl: '/app/views/home/home.html',
//            controller: 'homeController',
//            resolve: {
//                // Toutes les propriétés de “resolve” doivent retourner une promise
//                // et sont exécutuées avant que la vue ne soit chargée
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    // you can lazy load files for an existing module
//                    return $ocLazyLoad.load('home');
//                }]
//            }
//        })
//         .when('/inscriptions', {
//             templateUrl: '/app/views/home/inscriptions.html',
//             controller: 'homeController',
//             resolve: {
//                 loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                     return $ocLazyLoad.load('inscriptions');
//                 }]
//             }
//         })
//        .when('/inscription', {
//            templateUrl: '/app/views/inscription/inscription.html',
//            controller: 'inscriptionController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('inscription');
//                }]
//            }
//        })
//         .when('/mot-de-passe-oublie', {
//             templateUrl: '/app/views/inscription/motPasseOublie.html',
//             controller: 'inscriptionController',
//             resolve: {
//                 loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                     return $ocLazyLoad.load('mot-de-passe-oublie');
//                 }]
//             }
//         })
//        .when('/service', {
//            templateUrl: '/app/views/login/loginView.html',
//            controller: 'inscriptionController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('service');
//                }]
//            }
//        })
//        .when('/praticien', {
//            templateUrl: '/app/views/praticien/praticien.html',
//            controller: 'praticienController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('praticien');
//                }]
//            }
//        })
//        .when('/contact', {
//            templateUrl: '/app/views/contact/contact.html',
//            controller: 'contactController',
//            animate: "scrolling",
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('contact');
//                }]
//            }
//        })

//        .when('/inscrirepraticien', {
//            templateUrl: '/app/views/praticien/inscpriptionPraticien.html',
//            controller: 'inscriptionPraticienController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('inscrirepraticien');
//                }]
//            }
//        })
//       .when('/mesrendezvous', {
//           templateUrl: '/app/views/inscription/suivreMesRdv.html',
//           controller: 'suivreMesRdvController',
//           resolve: {
//               loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                   return $ocLazyLoad.load('mesrendezvous');
//               }]
//           }
//       })
//        .when('/praticiens', {
//            templateUrl: '/app/views/praticien/praticiens.html',
//            controller: 'praticienController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('praticiens');
//                }]
//            }
//        })
//       .when('/rendezvous', {
//           templateUrl: '/app/views/inscription/rendezvous.html',
//           controller: 'rendezVousController',
//           resolve: {
//               loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                   return $ocLazyLoad.load('rendezvous');
//               }]
//           }
//       })
//         .when('/confirmrendezvous', {
//             templateUrl: '/app/views/inscription/priserendezvous.html',
//             controller: 'priseRendezVousController',
//             resolve: {
//                 loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                     return $ocLazyLoad.load('confirmrendezvous');
//                 }]
//             }
//         })
//     .when('/loginpatient', {
//         templateUrl: '/app/views/login/loginpatient.html',
//         controller: 'inscriptionController',
//         resolve: {
//             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                 return $ocLazyLoad.load('loginpatient');
//             }]
//         }
//     })
//     .when('/mentionsLegales', {
//         templateUrl: '/app/views/contact/conditionsGenerales.html',
//         resolve: {
//             loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                 return $ocLazyLoad.load('mentionsLegales');
//             }]
//         }

//     })
//        .when('/medecins/:nomPrenom', {
//            templateUrl: '/app/views/medecins/pagemedecin.html',
//            controller: 'profilemedecinController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('medecins');
//                }]
//            }

//        })
//           .when('/monCompte', {
//               templateUrl: '/app/views/inscription/monCompte.html',
//               controller: 'monCompteController',
//               resolve: {
//                   loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                       return $ocLazyLoad.load('monCompte');
//                   }]
//               }

//           })
//         .when('/blog', {
//             templateUrl: '/app/views/contact/blog.html',
//             controller: 'blogController',
//             resolve: {
//                 loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                     return $ocLazyLoad.load('blog');
//                 }]
//             }

//         })
//        .otherwise({
//            templateUrl: '/app/views/home/home.html',
//            controller: 'homeController',
//            resolve: {
//                loadModule: ['$ocLazyLoad', function ($ocLazyLoad) {
//                    return $ocLazyLoad.load('home');
//                }]
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


var objectFromOutSide = {
    heurerdv: '',
    dateSelectionne: '',
    jour: '',
    praticien:''
};



var praticienInfo = 
{
    praticienEmail: '',
    nomPrenom : ''
};

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
app.run(['$http', '$rootScope', '$location', '$window', function ($http, $rootScope, $location, $window) {
    var username = getCookie("username");
    var password = getCookie("password");

    var basicAuth = window.btoa(username + ':' + password);
    $http.defaults.headers.common.Authorization = 'Basic ' + basicAuth;

    $rootScope
       .$on('$stateChangeSuccess',
           function (event) {

               if (!$window.ga)
                   return;

               $window.ga('send', 'pageview', { page: $location.path() });
           });

}]);


app.filter('shortDate', function () {
  
    return function (item) {
       
        return item.substring(0, 5);
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

app.directive('disableAnimation', function ($animate) {
    return {
        restrict: 'A',
        link: function ($scope, $element, $attrs) {
            $attrs.$observe('disableAnimation', function (value) {
                $animate.enabled(!value, $element);
            });
        }
    }
});
// App Settings
//var apiUri = 'http://localhost:60847/';
var apiUri = 'http://allotabib-api.allotabib.net/';

app.constant('appSettings', {
    praticienUri: apiUri + "praticien/", //avoir tous les praticiens
    praticienSearch: apiUri + "praticien",
    patientUri: apiUri + "patient/",
    contactUri: apiUri + "contact/",
    getPatientByEmail: apiUri + 'patient/getpatient',
    getPraticienByEmail: apiUri + 'praticien/getpraticien',
    getPraticienByNomPrenom: apiUri + 'praticien/getpraticienparnom',
    userIsAuth: apiUri + 'account/ispatient',
    userAccount: apiUri + 'account/',
    rendezvousUri: apiUri + 'calendriers/calendrier',
    calendrierUri: apiUri + 'calendriers/calendriersem',
    calendrierJourDisp: apiUri + 'calendriers/premieredisp',
    calendrierSemUri: apiUri + 'calendriers/calendriersemaine',
    rendezVousPatientUri: apiUri + 'rendezvous'
});

app.constant('defaultPageSize', {
    mainMaxPageSize: 50
});



