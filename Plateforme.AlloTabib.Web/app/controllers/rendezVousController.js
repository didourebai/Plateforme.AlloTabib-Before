'use strict';

app.controller('rendezVousController', function ($scope, $window, $location, praticienApiServices, contactApiServices, rendezvousApiServices) {
   
    var userName = $window.sessionStorage["userName"];

    if (userName == '' || userName == null || userName == undefined)
        $location.path('/loginpatient');
    
    //on ne peut pas sauvegarder un objet dans  $window.sessionStorage !!
    if (localStorage["praticienRdv"] != null && localStorage["praticienRdv"] != "undefined" && localStorage["praticienRdv"]!="") {
        $scope.PraticienSelectedToRdv = JSON.parse(localStorage["praticienRdv"]);

        if ($scope.PraticienSelectedToRdv.conventionne == 'True')
            $scope.estConventionne = true;
        else
            $scope.estConventionne = false;
    }
    
    initialize();
    
    afficherCalendrier();
    
    var map;
    var geocoder;
    function initialize() {
        $.blockUI();
        //Init map
        map = null;
     
        var latlng = new google.maps.LatLng($scope.PraticienSelectedToRdv.adresseGoogleLag, $scope.PraticienSelectedToRdv.adresseGoogleLong);
        var myOptions =
        {
            zoom: 16,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            disableDefaultUI: true
        };
        map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
        var marker = new google.maps.Marker({
            position: latlng,
            map: map,
            title: $scope.PraticienSelectedToRdv.adresse
        });



        var contentString = '<div id="content">'+
    '<div id="siteNotice">'+
    '</div>'+
    '<h4 id="firstHeading" class="firstHeading">'+$scope.PraticienSelectedToRdv.nomPrenom+'</h4>'+
    '<div id="bodyContent">'+
    '<p><b>' + $scope.PraticienSelectedToRdv.adresse + '</b></p>' +
    '</div>'+
    '</div>'+
    '</div>' +
    '</div>'
        ;

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

      
        google.maps.event.addListener(marker, 'click', function() {
            infowindow.open(map,marker);
        });
    
        google.maps.event.addDomListener(window, 'load', initialize);
       
    }

    //function initialize() {
         
    //    geocoder = new google.maps.Geocoder();
    //    initializeMap();

    //    var address = $scope.PraticienSelectedToRdv.adresse + " " + $scope.PraticienSelectedToRdv.gouvernerat + " Tunisie";
    //    geocoder.geocode({ 'address': address }, function (results, status) {
    //        if (status == google.maps.GeocoderStatus.OK) {
    //            map.setCenter(results[0].geometry.location);
    //            var marker = new google.maps.Marker({
    //                map: map,
    //                position: results[0].geometry.location
    //            });

    //        }
    //        else {
    //            alert("Geocode was not successful for the following reason: " + status);
    //        }
    //    });

    //}


    //afficher la liste des rendezvous de chaque médecins par semaine (7 jours)
    
    
    $scope.specialites = [
       'ANESTHESIE-REANIMATION',
       'CARCINOLOGIE MEDICALE',
       'CARDIOLOGIE',
       'CHI. CARDIOVASCULAIRE T',
       'CHIRURGIE CARCINOLOGIE',
       'CHIRURGIE GENERALE',
       'CHIRURGIE INFANTILE',
       'CHIRURGIE MAXILLO-FACIALE',
       'CHIRURGIE ORTHOPEDIQUE',
       'CHIRURGIE PLASTIQUE',
       'DERMATOLOGIE',
       'ENDOCRINOLOGIE',
       'GASTRO-HEPATHOLOGIE',
       'GYNECOLOGIE-OBSTETRIQUE',
       'HEMATOLOGIE',
       'MALADIES INFECTIEUSES',
       'MEDECINE GENERALE',
       'MEDECINE INTERNE',
       'MEDECINE LEGALE',
       'MEDECINE NUCLEAIRE',
       'MEDECINE PHYSIQUE',
       'NEUROCHIRURGIE',
       'NEUROLOGIE',
       'NUTRITION-DIETETIQUE',
       'OPHTALMOLOGIE',
       'ORL ET STOMATOLOGIE',
       'PARASITOLOGIE',
       'PEDIATRIE',
       'PNEUMOLOGIE',
       'PSYCHIATRIE',
       'RADIOLOGIE',
       'RHUMATOLOGIE',
       'UROLOGIE'
    ];
    $scope.gouvernerats = ['ARIANA', 'BEJA', 'BEN AROUS', 'BIZERTE', 'GABES', 'GAFSA', 'JENDOUBA', 'KAIROUAN', 'KASSERINE', 'KEBILI', 'KEF', 'MAHDIA', 'MANOUBA', 'MEDENINE', 'MONASTIR', 'NABEL', 'SFAX', 'SIDI BOUZID', 'SILIANA', 'SOUSSE', 'TATAOUINE', 'TOZEUR', 'TUNIS', 'ZAGHOUAN'];
    
    $scope.heuresDispoCalendrier = [];
   
    
    $scope.searchPraticien = function () {
        $.blockUI();
        if ($scope.search == undefined)
        {
            $scope.search = {
                specialite: undefined,
                gouvernerat: undefined,
                nomPrenom: undefined
            };
        }
        if ($scope.search.specialite != undefined) {
            $scope.getSubQuery = $scope.search.specialite;

        }

        if ($scope.search.gouvernerat != undefined) {
            $scope.getSubQuery = $scope.getSubQuery + " " + $scope.search.gouvernerat;

        }

        if ($scope.search.nomPrenom != undefined) {
            $scope.getSubQuery = $scope.getSubQuery + " " + $scope.search.nomPrenom;

        }

        $.blockUI();

        praticienApiServices.searchPraticien($scope.search.gouvernerat, $scope.search.specialite, $scope.search.nomPrenom, 0, 0 /*$scope.pageSize, ($scope.currentPage - 1) * $scope.pageSize*/)
                   .then
                    (
                        // Case : Success.
                        function (result) {
                            // avoir la liste des praticiens
                            $scope.praticiensList = result.data.data.items;

                            localStorage["praticicensResult"] = JSON.stringify($scope.praticiensList);
                            $window.sessionStorage["listPraticiens"] = $scope.praticiensList;

                            // check if the list of board is not empty.
                            if ($scope.praticiensList.length > 0) {

                                // Get the pagination parameters (total items count and pages count)
                                var paginationHeader = result.data.data.paginationHeader;

                                // Set the pagination parameters.
                                $scope.totalRecordsCount = paginationHeader.totalCount;
                                $scope.totalPages = paginationHeader.totalPages;

                            }
                            else {
                                // If the list of board is empty, this message will be shown.
                                //$scope.transactionState = 'No items found.';
                                //TODO ajouter un message ici en popup
                            }

                            $location.path('/praticiens');
                            // Stop the progress bar.
                            $.unblockUI();
                        },

                        // Case : Error.
                        function (error) {

                            if (error.data != null && error.data != "") {

                                if (error.status == 500) {
                                    if (error.data.errors != null && error.data.errors[0] != null && error.data.errors[0].exception != null)
                                        $scope.transactionState = error.data.errors[0].exception.Message;
                                }
                                else {
                                    if (error.data.errors != null && error.data.errors[0] != null && error.data.errors[0].message != null)
                                        $scope.transactionState = error.data.errors[0].message;
                                }
                            }
                            else {
                                $scope.transactionState = 'InternalServerError';
                                $scope.errorMessage = 'Veuillez nous contacter pour ce problème. Equipe AlloTabib';
                            }

                            // Stop the progress bar.
                            $.unblockUI();
                        }
                    );
    };
    
    function formattedDate(date) {
        $.blockUI();
        var d = new Date(date || Date.now()),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [day, month, year].join('/');
    }

    $scope.heureDispoCalendrier = {
        dateCourante: "",
        jour: "",
        heureCalendrier: []
    };
    $scope.previousWeek = function() {
        $.blockUI();
        $scope.data = null;
        var praticienCin = $scope.PraticienSelectedToRdv.email;
        rendezvousApiServices.calendrierPourPatientPrevious(praticienCin, $scope.previousDay.dateCourante).then(
              // Case : Success.
         function (results) {

             
             var datas = results.data.data;
             $scope.data = datas;
             //$scope.heuresDispoCalendrier = datas;
             $scope.heuresDispoCalendrier = datas;
             $scope.lastDay = datas[datas.length - 1];
             $scope.previousDay = datas[0];

             $.unblockUI();
         },
              // Case : Error.
         function (error) {
             $.unblockUI();
             //TODO gérer les exceptions avec l'api
             if (error != null && error.data != null && error.data.errors != null && error.data.errors[0].message != null) {
                 
                 toastr.warning(error.data.errors[0].message);
             } else {
                 toastr.error("Problème avec la  plateforme, veuillez contacter AlloTabib.");
             }
         });
  

    };

    $scope.nextWeek = function() {
        $.blockUI();
      
        var praticienCin = $scope.PraticienSelectedToRdv.email;
        rendezvousApiServices.calendrierPourPatient(praticienCin, $scope.lastDay.dateCourante).then(
              // Case : Success.
         function (results) {

             //Refaire la liste comme dans le lien 
             //http://plnkr.co/edit/e41n9vAMMLf0WWIUQ8HO?p=preview
             //http://stackoverflow.com/questions/16485274/using-ng-repeat-on-json-containing-json
             var datas = results.data.data;
             //$scope.heuresDispoCalendrier = datas;
             $scope.heuresDispoCalendrier = datas;
             $scope.lastDay = datas[datas.length - 1];
             $scope.previousDay = datas[0];

             $.unblockUI();
         },
              // Case : Error.
         function (error) {
             //TODO gérer les exceptions avec l'api
             toastr.error("Problème avec la  plateforme, veuillez contacter AlloTabib.");
             $.unblockUI();
         });

    };

    $scope.praticienInfor = function (mail, nomPrenom) {
        
        praticienInfo.praticienEmail = mail;
        praticienInfo.nomPrenom = nomPrenom.replace(" ",".");

        $window.open('index.html#/medecins/' + praticienInfo.nomPrenom);
    };

    $scope.lastDay = new Date();
    $scope.previousDay = new Date();
    
    function afficherCalendrier() {
       $.blockUI();
        var praticienCin = $scope.PraticienSelectedToRdv.email;
        var today = new Date();
      
        rendezvousApiServices.calendrierPourPatient(praticienCin, formattedDate(today)).then(
                // Case : Success.
           function (results) {
               
               //Refaire la liste comme dans le lien 
               //http://plnkr.co/edit/e41n9vAMMLf0WWIUQ8HO?p=preview
               //http://stackoverflow.com/questions/16485274/using-ng-repeat-on-json-containing-json
               var datas = results.data.data;
               
               //$scope.heuresDispoCalendrier = datas;
               $scope.heuresDispoCalendrier = datas;
               $scope.lastDay = datas[datas.length - 1];
               $scope.previousDay = datas[0];
               
               $.unblockUI();
            },
                // Case : Error.
           function (error) {
               //TODO gérer les exceptions avec l'api
               $.unblockUI();
           });

    }

    $scope.demandeConfirmeRdv = function (heure, dateSelectionne, jour, praticien) {
        
        objectFromOutSide.heurerdv = heure;
        objectFromOutSide.dateSelectionne = dateSelectionne;
        objectFromOutSide.jour = jour;
        objectFromOutSide.praticien = praticien;

        $location.path('/confirmrendezvous');
    };
  
    
    $scope.loadClasstableHeures = function (heureDispoCalendrier) {
        if (heureDispoCalendrier.heureCalendrier != null) {

            return 'tableHeuresActiveJ';
        } else {
            return 'tableHeuresNonActiveJ';
        }
    };

    $scope.loadClasstableJourNonActive = function (heureDispoCalendrier) {
        if (heureDispoCalendrier.heureCalendrier != null) {

            return 'tableJourActive';
        } else {
            return 'tableJourNonActive';
        }
    };
    
    $scope.loadClassTdActiveHeure = function (heureDispoCalendrier) {
        if (heureDispoCalendrier.heureCalendrier != null) {
            return 'tdActiveHeure';
        } else
            return 'tdNActiveHeure';
    };

    
});