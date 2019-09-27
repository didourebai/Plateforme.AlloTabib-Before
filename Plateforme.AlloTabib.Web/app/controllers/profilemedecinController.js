'use strict';
app.controller('profilemedecinController', function ($scope, $location, $window, praticienApiServices, rendezvousApiServices) {

    initialize();

    var map;
    var geocoder;

    $scope.isConnected = false;
    $scope.IsshowButtonRdv = false;
    function initialize() {

        $.blockUI();

        $scope.rendezVouspris = true;
        var userName = $window.sessionStorage["userName"];
        if (userName == undefined) {
            //hide button from the beginning
            $scope.isConnected = false;
        } else {
            $scope.isConnected = true;
        }

       
            if ($scope.rendezVouspris == true && $scope.isConnected == true) {
                $scope.IsshowButtonRdv = true;
            } else {
                $scope.IsshowButtonRdv = false;
            }
       
        if (praticienInfo != null && praticienInfo.praticienEmail != "" && praticienInfo.nomPrenom !="") {
            
            //récupérer tout les infos d'un praticien
            praticienApiServices.getPraticienByEmail(praticienInfo.praticienEmail).then(
                // Case : Success.
                function(results) {
                    $.unblockUI();

                    $scope.praticien = results.data.data;
                    showGoogleMaps($scope.praticien);
                    
                    //afficherCalendrier();
                },
                // Case : Error.
                function(error) {
                    $.unblockUI();
                    //toastr.error("Problème de récupération des informations d'un praticien, veuillez nous contacter par mail : contact@allotabib.net");
                }
            );

        } else {
            //Récupérer la page de l'extérieur par son nom getPraticienByNomPrenom
            var url = document.URL;
            var splitUrl = url.split('/');

            var nomPr = splitUrl[splitUrl.length - 1];
            
            nomPr = nomPr.replace(".", " ");
            console.log(nomPr);
            //récupérer tout les infos d'un praticien
            praticienApiServices.getPraticienByNomPrenom(nomPr).then(
                // Case : Success.
                function (results) {
                    $.unblockUI();

                    $scope.praticien = results.data.data;

                    showGoogleMaps($scope.praticien);
                    
                    //afficherCalendrier();
                },
                // Case : Error.
                function (error) {
                    $.unblockUI();
                    toastr.error("Problème de récupération des informations d'un praticien, veuillez nous contacter par mail : contact@allotabib.net");
                }
            );

        }
           
      
    }

    function showGoogleMaps(praticien) {
        $.blockUI();
        //Init map
        map = null;

        var latlng = new google.maps.LatLng(praticien.adresseGoogleLag, praticien.adresseGoogleLong);
        var myOptions =
        {
            zoom: 32,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            disableDefaultUI: true
        };
        map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
        var marker = new google.maps.Marker({
            position: latlng,
            map: map,
            title: praticien.adresse
        });



        var contentString = '<div id="content">' +
    '<div id="siteNotice">' +
    '</div>' +
    '<h4 id="firstHeading" class="firstHeading">' + praticien.nomPrenom + '</h4>' +
    '<div id="bodyContent">' +
    '<p><b>' + praticien.adresse + '</b></p>' +
    '</div>' +
    '</div>' +
    '</div>' +
    '</div>'
        ;

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });


        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map, marker);
        });

        google.maps.event.addDomListener(window, 'load', initialize);
        $.unblockUI();
    }

    $scope.previousWeek = function () {
        $.blockUI();
        $scope.data = null;
        var praticienCin = $scope.praticien.email;
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



        $.unblockUI();
    };

    $scope.nextWeek = function () {
        $.blockUI();

        var praticienCin = $scope.praticien.email;
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

    $scope.afficherCalendrier = function () {

        var url = document.URL;
        var splitUrl = url.split('/');

        var nomPr = splitUrl[splitUrl.length - 1];

        nomPr = nomPr.replace(".", " ");;

        $scope.search = nomPr;
        //Redirection à la prise de rendez vous en affichant le médecin concerné
        praticienApiServices.searchPraticien('', '', $scope.search, 0, 0 /*$scope.pageSize, ($scope.currentPage - 1) * $scope.pageSize*/)
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
                               window.scrollTo(0, 0);

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

    }
    function afficherCalendrier() {
        $.blockUI();
        var praticienCin = $scope.praticien.email;
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

    function formattedDate(date) {
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
});