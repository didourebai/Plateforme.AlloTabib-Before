'use strict';

app.controller('homeController', function ($scope, $location, patientApiServices, authService, praticienApiServices,contactApiServices, $window) {

    $scope.City = "";
    function getUnique(inputArray) {
        var outputArray = [];

        for (var i = 0; i < inputArray.length; i++) {
            if ((jQuery.inArray(inputArray[i], outputArray)) == -1) {
                outputArray.push(inputArray[i]);
            }
        }

        return outputArray;
    }


    /* Debut localisation */
    var options = {
        enableHighAccuracy: true,
        timeout: 5000,
        maximumAge: 0
    };

    function error(err) {
     console.warn('ERROR(' + err.code + '): ' + err.message);
    };

    function success(pos) {
        var crd = pos.coords;

        localisation(crd);
    }

   
    function localisation(crd) {
        var city = '';
        //Init map
        var map = null;

        var latlng = new google.maps.LatLng(crd.latitude, crd.longitude);
        var myOptions =
        {
            zoom: 16,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            disableDefaultUI: true
        };
    //    map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
    //    var marker = new google.maps.Marker({
    //        position: latlng,
    //        map: map,
    //        title: crd.adresse
    //    });



    //    var contentString = '<div id="content">' +
    //'<div id="siteNotice">' +
    //'</div>' +
    //'<h4 id="firstHeading" class="firstHeading">' + "Vous êtes ICI!!!" + '</h4>' +
    //'<div id="bodyContent">' +
    //'<p><b>' + crd.adresse + '</b></p>' +
    //'</div>' +
    //'</div>' +
    //'</div>' +
    //'</div>'
    //    ;

    //    var infowindow = new google.maps.InfoWindow({
    //        content: contentString
    //    });


    //    google.maps.event.addListener(marker, 'click', function () {
    //        infowindow.open(map, marker);
    //    });

        //google.maps.event.addDomListener(window, 'load', initialize);

        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'latLng': latlng }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {

                if (results[1]) {
                    //formatted address

                    //find country name

                    for (var i = 0; i < results[0].address_components.length; i++) {
                        for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                            //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                            if (results[0].address_components[i].types[b] == "administrative_area_level_1") {
                               
                                //this is the object you are looking for
                               
                                city = results[0].address_components[3].long_name;
                                
                                $scope.City = city;
                                break;
                            }
                        }
                    }
                } else {
                    console.log("No results found");
                }
            } else {
                console.log("Geocoder failed due to: " + status);
            }
        });
    }

   
    //on traite la localisation ici
    //getCurrentPosition
    navigator.geolocation.getCurrentPosition(success, error, options);

    //getSpecialities();
    $scope.specialites = [];
    $scope.specialitesInit = [];
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
      'UROLOGIE',
        'DENTISTE'
    ];
    $scope.gouverneratsInit = [];
    $scope.gouvernerats = [];
    $scope.gouvernerats = ['ARIANA', 'BEJA', 'BEN AROUS', 'BIZERTE', 'GABES', 'GAFSA', 'JENDOUBA', 'KAIROUAN', 'KASSERINE', 'KEBILI', 'KEF', 'MAHDIA', 'MANOUBA', 'MEDENINE', 'MONASTIR', 'NABEL', 'SFAX', 'SIDI BOUZID', 'SILIANA', 'SOUSSE', 'TATAOUINE', 'TOZEUR', 'TUNIS', 'ZAGHOUAN'];
    $scope.search = {        
        specialite : "",
        gouvernerat: "",
        searchApp : ""
    };

    //$scope.statesInputChanged = function () {
    //    alert($scope.search.specialite);
    //};
    //$scope.stateInputChanged = function () {
    //    if ($scope.search.gouvernerat != undefined)
    //        alert($scope.search.gouvernerat);
    //    if ($scope.search.specialite != undefined)
    //        alert($scope.search.specialite);
        

    //};
    
    function getSpecialities() {
        $.blockUI();
        praticienApiServices.getSpecialitiesGouvernerat().then(
            // Case : Success.
            function (result) {
                // Stop the progress bar.
                $.unblockUI();

                var specialities = result.data;
                var list = [];
                var gouv = [];
                
                for (var spe = 0; spe<specialities.data.length; spe++) {

                    var obj = specialities.data[spe];
                    list.push(obj.specialite);
                    gouv.push(obj.gouvernerat);
                }
                
                $scope.specialitesInit = getUnique(list);
                $scope.gouverneratsInit = getUnique(gouv);
                
                $scope.specialites = $scope.specialitesInit;
                $scope.gouvernerats = $scope.gouverneratsInit;
            },
             // Case : Error.
            function (error) {
               // Stop the progress bar.
                $.unblockUI();
                

            }
        );
    }
    
    $scope.praticiensList = [];
    //recherche par spécialité pour pied de page
    $scope.searchBySpeciality = function (specialite) {
        $.blockUI();
        $scope.search.specialite = specialite;
        $scope.searchPraticien();
        
        //$.unblockUI();
    };

    $scope.searchPraticien = function () {
        $.blockUI();
        if ($scope.search.specialite != undefined)
        {
            $scope.getSubQuery = $scope.search.specialite;
           
        }
            
        if ($scope.search.gouvernerat != undefined)
        {
            $scope.getSubQuery = $scope.getSubQuery + " " + $scope.search.gouvernerat;
          
        }
            
        if ($scope.search.searchApp != undefined)
        {
            $scope.getSubQuery = $scope.getSubQuery + " " + $scope.search.searchApp;
           
        }

   

        if ($scope.search.gouvernerat == "") {
            $.unblockUI();
            
            if ($scope.City == '') {
                $.unblockUI();
                toastr.warning("Veuillez choisir une des critères : spécialité ou gouvernerat.");
            } else {
                $.unblockUI();
                toastr.success("Suivant votre localisation, vous aurez les médecins proche à vous dans  : " + $scope.City + ", Si le gouvernerat n'existe pas, vous aurez toute la liste.");
                if ($scope.gouvernerats.indexOf($scope.City) != -1)
                {
                    $scope.search.gouvernerat = $scope.City;
                }
                $scope.search.gouvernerat = "";
            }

        }
       
        if ($scope.search.specialite == "" && $scope.search.gouvernerat == "" && $scope.search.searchApp == "" && $scope.City == '') {

            toastr.error("Veuillez choisir au moins un critère : spécialité ou gouvernerat.");
            

        } else {

            localStorage["searchParam"] = JSON.stringify($scope.search);
            $location.path('/praticiens');
        }

        //}
       
    };
    function sendEmail(contactObject) {
        
        contactApiServices.sendEmail(contactObject)
           .then(
               // Case : Success.
               function (results) {
                   $.unblockUI();
                   $scope.newContact = {};
                   
                   $scope.isSuccessfullyAdded = true;
                   $scope.addHasErrors = false;
                   toastr.success("Nous vous contacterons dès que possible.");
               },
               // Case : Error.
               function (error) {
                   $.unblockUI();
                   //Afficher panel des erreurs
                   $scope.addHasErrors = true;
                   //ne pas afficher le message de succès
                   $scope.isSuccessfullyAdded = false;

                   // Add the errors messages.

                   if (error.status == 400) {
                       //if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                       //    $scope.addErrorType = "Erreur de validation";
                       //    $scope.errorMessageTxt = error.data.errors[0].message;
                       //}
                   }

                   if (error.status == 401) {
                       //$scope.addErrorMessage = JSON.parse(error.data);
                       //$scope.errorMessageTxt = "Non autorisé";
                   }

                   if (error.status == 500) {
                       //$scope.addErrorType = "Erreur lié au serveur";
                       //$scope.errorMessageTxt = error.data.errors[0].exception.Message;
                   }
                   //$scope.addErrorType = "Erreur anonyme";
                   //$scope.errorMessageTxt = error.data.toString();
               }
           );
    }

    $scope.sendPraticienEmail = function () {
        
        if ($scope.praticien == undefined)
        {
            toastr.error("Veuillez Dr saisir votre email / téléphone pour vous contacter !");
        }
        else
        {
            if($scope.praticien.email =='' || $scope.praticien.email == undefined)
            {
                toastr.error("Veuillez Dr saisir votre email pour vous contacter !");
            }
            else 
            if ($scope.praticien.telephone == '' || $scope.praticien.telephone == undefined) {
                toastr.error("Veuillez Dr saisir votre téléphone pour vous contacter !");
            }else
            {
                //envoyer un email à moi 
                $scope.contactObject = {
                    from: "contact@allotabib.net",
                    to: "didourebai@gmail.com",
                    sujet: "[AlloTabib] Question d'un praticien",
                    body: "Bonjour, Merci de me contactez j'ai des questions." + "téléphone :" + $scope.praticien.telephone + "Email :" + $scope.praticien.email
                };

                sendEmail($scope.contactObject);
            }
        }
        
    };

    $scope.sendEmail = function () {
    
        if ($scope.patient == undefined)
        {
            toastr.error("Veuillez saisir votre email pour vous contacter !");

        } else

        {
            if ($scope.patient.email == '' || $scope.patient.email == undefined) {
                toastr.error("Veuillez Dr saisir votre email pour vous contacter !");
            }
            else
                if ($scope.patient.message == '' || $scope.patient.message == undefined) {
                    toastr.error("Veuillez Dr saisir votre question pour vous répondre !");
                } else {
                    //envoyer un email à moi 
                    $scope.contactObject = {
                        from: "contact@allotabib.net",
                        to: "didourebai@gmail.com",
                        sujet: "[AlloTabib] Question d'un praticien",
                        body: "Bonjour, Merci de me contactez j'ai des questions." + "question :" + $scope.patient.message + "email : " + $scope.patient.email
                    };
                    $.blockUI();
                    sendEmail($scope.contactObject);
                }
        }
        
    };

});