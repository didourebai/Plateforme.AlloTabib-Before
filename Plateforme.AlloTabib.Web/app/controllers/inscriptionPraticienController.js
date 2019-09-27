'use strict';

app.controller('inscriptionPraticienController', function ($scope, $location, praticienApiServices, contactApiServices, $window) {

    // Initialization
    $scope.showValidationErrorForTitle = true;
    $scope.EstEninscriptionPatient = true;
    $scope.isSuccessfullyAdded = false;
    $scope.addHasErrors = false;
    $scope.clicked = false;
   


    $scope.estConventionne = function (praticien) {

        if (praticien == 'True') {
            return true;
        }
        return false;
    };

    $scope.forgetPassword = function () {
        $('#forgetPassword').modal({
            backdrop: 'static',
            keyboard: false
        });

    };

    $scope.getTitle = function (param) {
        return '<b>' + param + '</b>';
    };

    $scope.validerEmail = function () {
        //initialiser le champs
        $scope.forgetPasswordEmail = null;
        $scope.showValidationErrorForTitle = false;
    };

    $scope.cancelForgetPassword = function () {
        //initialiser le champs
        $scope.forgetPasswordEmail = null;
        $scope.showValidationErrorForTitle = false;
    };
    $scope.creerComptePraticien = function () {
        $location.path('/inscrirepraticien');
    };

    $scope.loadInputClassForAdd = function (valid) {

        if ($scope.clicked == false) {
            return 'is-not-clicked';
        }
        if (valid) {
            return 'has-success';
        } else {
            return 'has-error';
        }
    };

    $scope.Isclicked = function () {
        $scope.clicked = true;
    };

    $scope.heures = [
        { id: '0', value: '0', text: "00" },
        { id: '1', value: '1', text: "01" },
        { id: '2', value: '2', text: "02" },
        { id: '3', value: '3', text: "03" },
        { id: '4', value: '4', text: "04" },
        { id: '5', value: '5', text: "05" },
        { id: '6', value: '6', text: "06" },
        { id: '7', value: '7', text: "07" },
        { id: '8', value: '8', text: "08" },
        { id: '9', value: '9', text: "09" },
        { id: '10', value: '10', text: "10" },
        { id: '11', value: '11', text: "11" },
        { id: '12', value: '12', text: "12" },
        { id: '13', value: '13', text: "13" },
        { id: '14', value: '14', text: "14" },
        { id: '15', value: '15', text: "15" },
        { id: '16', value: '16', text: "16" },
        { id: '17', value: '17', text: "17" },
        { id: '18', value: '18', text: "18" },
        { id: '19', value: '19', text: "19" },
        { id: '20', value: '20', text: "20" },
        { id: '21', value: '21', text: "21" },
        { id: '22', value: '22', text: "22" },
        { id: '23', value: '23', text: "23" }
    ];
    $scope.minutes = [];
    $scope.integerval = /^\d*$/;

    function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }
    for (var mn = 0; mn < 60; mn++) {
        mn = addZero(mn.toString());
        $scope.minutes.push(mn);
    }
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


    $scope.gouvernerats = ['ARIANA', 'BEJA', 'BEN AROUS', 'BIZERTE', 'GABES', 'GAFSA', 'JENDOUBA', 'KAIROUAN', 'KASSERINE', 'KEBILI', 'KEF', 'MAHDIA', 'MANOUBA', 'MEDENINE', 'MONASTIR', 'NABEL', 'SFAX', 'SIDI BOUZID', 'SILIANA', 'SOUSSE', 'TATAOUINE', 'TOZEUR', 'TUNIS', 'ZAGHOUAN'];
    $scope.delegations = [
        { gouv: 'ARIANA', value: 'RAOUED' },
        { gouv: 'ARIANA', value: 'SIDI THABET' },
        { gouv: 'ARIANA', value: 'ARIANA VILLE' },
        { gouv: 'ARIANA', value: 'LA SOUKRA' },
        { gouv: 'ARIANA', value: 'KALAAT LANDLOUS' },
        { gouv: 'ARIANA', value: 'ETTADHAMEN' },
        { gouv: 'ARIANA', value: 'MNIHLA' },

        { gouv: 'BEJA', value: 'BEJA NORD' },
        { gouv: 'BEJA', value: 'NEFZA' },
        { gouv: 'BEJA', value: 'GOUBELLAT' },
        { gouv: 'BEJA', value: 'MEJEZ EL BAB' },
        { gouv: 'BEJA', value: 'AMDOUN' },
        { gouv: 'BEJA', value: 'TEBOURSOUK' },
        { gouv: 'BEJA', value: 'TESTOUR' },
        { gouv: 'BEJA', value: 'THIBAR' },
        { gouv: 'BEJA', value: 'BEJA SUD' },

         { gouv: 'BEN AROUS', value: 'EZZAHRA' },
         { gouv: 'BEN AROUS', value: 'MOHAMADIA' },
         { gouv: 'BEN AROUS', value: 'RADES' },
         { gouv: 'BEN AROUS', value: 'EL MOUROUJ' },
         { gouv: 'BEN AROUS', value: 'FOUCHANA' },
         { gouv: 'BEN AROUS', value: 'HAMMAM CHATT' },
         { gouv: 'BEN AROUS', value: 'HAMMAM LIF' },
         { gouv: 'BEN AROUS', value: 'MEGRINE' },
         { gouv: 'BEN AROUS', value: 'NOUVELLE MEDINA' },
         { gouv: 'BEN AROUS', value: 'MORNAG' },
         { gouv: 'BEN AROUS', value: 'BOU MHEL EL BASSATINE' },
         { gouv: 'BEN AROUS', value: 'BEN AROUS' },

         { gouv: 'BIZERTE', value: 'MENZEL BOURGUIBA' },
         { gouv: 'BIZERTE', value: 'UTIQUE' },
         { gouv: 'BIZERTE', value: 'MENZEL JEMIL' },
         { gouv: 'BIZERTE', value: 'BIZERTE NORD' },
         { gouv: 'BIZERTE', value: 'BIZERTE SUD' },
         { gouv: 'BIZERTE', value: 'EL ALIA' },
         { gouv: 'BIZERTE', value: 'SEJNANE' },
         { gouv: 'BIZERTE', value: 'GHAR EL MELH' },
         { gouv: 'BIZERTE', value: 'GHEZALA' },
         { gouv: 'BIZERTE', value: 'JARZOUNA' },
         { gouv: 'BIZERTE', value: 'JOUMINE' },
         { gouv: 'BIZERTE', value: 'MATEUR' },
         { gouv: 'BIZERTE', value: 'RAS JEBEL' },
         { gouv: 'BIZERTE', value: 'TINJA' },

         { gouv: 'GABES', value: 'EL METOUIA' },
         { gouv: 'GABES', value: 'GABES MEDINA' },
         { gouv: 'GABES', value: 'GABES OUEST' },
         { gouv: 'GABES', value: 'GABES SUD' },
         { gouv: 'GABES', value: 'EL HAMMA' },
         { gouv: 'GABES', value: 'NOUVELLE MATMATA' },
         { gouv: 'GABES', value: 'MARETH' },
         { gouv: 'GABES', value: 'GHANNOUCHE' },
         { gouv: 'GABES', value: 'MATMATA' },
         { gouv: 'GABES', value: 'MENZEL HABIB' },

        { gouv: 'GAFSA', value: 'BELKHIR' },
        { gouv: 'GAFSA', value: 'EL GUETTAR' },
        { gouv: 'GAFSA', value: 'EL KSAR' },
        { gouv: 'GAFSA', value: 'EL MDHILLA' },
        { gouv: 'GAFSA', value: 'SNED' },
        { gouv: 'GAFSA', value: 'MOULARES' },
        { gouv: 'GAFSA', value: 'REDEYEF' },
        { gouv: 'GAFSA', value: 'SIDI AICH' },
        { gouv: 'GAFSA', value: 'GAFSA SUD' },
        { gouv: 'GAFSA', value: 'METLAOUI' },
        { gouv: 'GAFSA', value: 'GAFSA NORD' },

        { gouv: 'JENDOUBA', value: 'FERNANA' },
        { gouv: 'JENDOUBA', value: 'GHARDIMAOU' },
        { gouv: 'JENDOUBA', value: 'TABARKA' },
        { gouv: 'JENDOUBA', value: 'JENDOUBA' },
        { gouv: 'JENDOUBA', value: 'JENDOUBA NORD' },
        { gouv: 'JENDOUBA', value: 'AIN DRAHAM' },
        { gouv: 'JENDOUBA', value: 'OUED MLIZ' },
        { gouv: 'JENDOUBA', value: 'BOU SALEM' },
        { gouv: 'JENDOUBA', value: 'BALTA BOU AOUENE' },

           { gouv: 'KAIROUAN', value: 'SBIKHA' },
           { gouv: 'KAIROUAN', value: 'KAIROUAN SUD' },
           { gouv: 'KAIROUAN', value: 'OUESLATIA' },
           { gouv: 'KAIROUAN', value: 'NASRALLAH' },
           { gouv: 'KAIROUAN', value: 'KAIROUAN NORD' },
           { gouv: 'KAIROUAN', value: 'EL ALA' },
           { gouv: 'KAIROUAN', value: 'HAJEB EL AYOUN' },
           { gouv: 'KAIROUAN', value: 'CHEBIKA' },
           { gouv: 'KAIROUAN', value: 'HAFFOUZ' },
           { gouv: 'KAIROUAN', value: 'CHERARDA' },
           { gouv: 'KAIROUAN', value: 'BOU HAJLA' },

           { gouv: 'KASSERINE', value: 'HAIDRA' },
          { gouv: 'KASSERINE', value: 'SBEITLA' },
          { gouv: 'KASSERINE', value: 'MEJEL BEL ABBES' },
          { gouv: 'KASSERINE', value: 'KASSERINE NORD' },
          { gouv: 'KASSERINE', value: 'EL AYOUN' },
          { gouv: 'KASSERINE', value: 'EZZOUHOUR' },
          { gouv: 'KASSERINE', value: 'FERIANA' },
          { gouv: 'KASSERINE', value: 'FOUSSANA' },
          { gouv: 'KASSERINE', value: 'HASSI EL FRID' },
          { gouv: 'KASSERINE', value: 'JEDILIANE' },
          { gouv: 'KASSERINE', value: 'KASSERINE SUD' },
          { gouv: 'KASSERINE', value: 'SBIBA' },
          { gouv: 'KASSERINE', value: 'THALA' },

        { gouv: 'KEBILI', value: 'SOUK EL AHAD' },
        { gouv: 'KEBILI', value: 'KEBILI SUD' },
        { gouv: 'KEBILI', value: 'KEBILI NORD' },
        { gouv: 'KEBILI', value: 'DOUZ' },
        { gouv: 'KEBILI', value: 'EL FAOUAR' },

        { gouv: 'KEF', value: 'TAJEROUINE' },
        { gouv: 'KEF', value: 'TOUIREF' },
        { gouv: 'KEF', value: 'NEBEUR' },
        { gouv: 'KEF', value: 'SAKIET SIDI YOUSSEF' },
        { gouv: 'KEF', value: 'LE SERS' },
        { gouv: 'KEF', value: 'EL KSOUR' },
        { gouv: 'KEF', value: 'LE KEF EST' },
        { gouv: 'KEF', value: 'DAHMANI' },
        { gouv: 'KEF', value: 'KALAAT SINANE' },
        { gouv: 'KEF', value: 'JERISSA' },
        { gouv: 'KEF', value: 'KALAA EL KHASBA' },
        { gouv: 'KEF', value: 'LE KEF OUEST' },

        { gouv: 'MAHDIA', value: 'MELLOULECH' },
        { gouv: 'MAHDIA', value: 'SIDI ALOUENE' },
        { gouv: 'MAHDIA', value: 'MAHDIA' },
        { gouv: 'MAHDIA', value: 'SOUASSI' },
        { gouv: 'MAHDIA', value: 'OULED CHAMAKH' },
        { gouv: 'MAHDIA', value: 'BOU MERDES' },
        { gouv: 'MAHDIA', value: 'CHORBANE' },
        { gouv: 'MAHDIA', value: 'KSOUR ESSAF' },
        { gouv: 'MAHDIA', value: 'HBIRA' },
        { gouv: 'MAHDIA', value: 'LA CHEBBA' },
        { gouv: 'MAHDIA', value: 'EL JEM' },

                { gouv: 'MANOUBA', value: 'BORJ EL AMRI' },
                { gouv: 'MANOUBA', value: 'JEDAIDA' },
                { gouv: 'MANOUBA', value: 'OUED ELLIL' },
                { gouv: 'MANOUBA', value: 'TEBOURBA' },
                { gouv: 'MANOUBA', value: 'EL BATTAN' },
                { gouv: 'MANOUBA', value: 'MANNOUBA' },
                { gouv: 'MANOUBA', value: 'MORNAGUIA' },
                { gouv: 'MANOUBA', value: 'DOUAR HICHER' },

         { gouv: 'MEDENINE', value: 'HOUMET ESSOUK' },
        { gouv: 'MEDENINE', value: 'MEDENINE SUD' },
        { gouv: 'MEDENINE', value: 'BENI KHEDACHE' },
        { gouv: 'MEDENINE', value: 'SIDI MAKHLOUF' },
        { gouv: 'MEDENINE', value: 'MIDOUN' },
        { gouv: 'MEDENINE', value: 'ZARZIS' },
        { gouv: 'MEDENINE', value: 'MEDENINE NORD' },
        { gouv: 'MEDENINE', value: 'BEN GUERDANE' },
        { gouv: 'MEDENINE', value: 'AJIM' },

           { gouv: 'MONASTIR', value: 'SAYADA LAMTA BOU HAJAR' },
           { gouv: 'MONASTIR', value: 'KSIBET EL MEDIOUNI' },
           { gouv: 'MONASTIR', value: 'KSAR HELAL' },
           { gouv: 'MONASTIR', value: 'JEMMAL' },
           { gouv: 'MONASTIR', value: 'SAHLINE' },
           { gouv: 'MONASTIR', value: 'MONASTIR' },
           { gouv: 'MONASTIR', value: 'MOKNINE' },
           { gouv: 'MONASTIR', value: 'OUERDANINE' },
           { gouv: 'MONASTIR', value: 'TEBOULBA' },
           { gouv: 'MONASTIR', value: 'ZERAMDINE' },
           { gouv: 'MONASTIR', value: 'BEKALTA' },
           { gouv: 'MONASTIR', value: 'BEMBLA' },
           { gouv: 'MONASTIR', value: 'BENI HASSEN' },

         { gouv: 'NABEL', value: 'KORBA' },
         { gouv: 'NABEL', value: 'SOLIMAN' },
         { gouv: 'NABEL', value: 'TAKELSA' },
         { gouv: 'NABEL', value: 'MENZEL BOUZELFA' },
         { gouv: 'NABEL', value: 'MENZEL TEMIME' },
         { gouv: 'NABEL', value: 'NABEUL' },
         { gouv: 'NABEL', value: 'BENI KHIAR' },
         { gouv: 'NABEL', value: 'BENI KHALLED' },
         { gouv: 'NABEL', value: 'HAMMAMET' },
         { gouv: 'NABEL', value: 'EL HAOUARIA' },
         { gouv: 'NABEL', value: 'KELIBIA' },
         { gouv: 'NABEL', value: 'GROMBALIA' },
         { gouv: 'NABEL', value: 'EL MIDA' },
         { gouv: 'NABEL', value: 'BOU ARGOUB' },
         { gouv: 'NABEL', value: 'DAR CHAABANE ELFEHRI' },
         { gouv: 'NABEL', value: 'HAMMAM EL GHEZAZ' },

        { gouv: 'SFAX', value: 'EL HENCHA' },
        { gouv: 'SFAX', value: 'ESSKHIRA' },
        { gouv: 'SFAX', value: 'GHRAIBA' },
        { gouv: 'SFAX', value: 'EL AMRA' },
        { gouv: 'SFAX', value: 'BIR ALI BEN KHELIFA' },
        { gouv: 'SFAX', value: 'AGAREB' },
        { gouv: 'SFAX', value: 'SFAX VILLE' },
        { gouv: 'SFAX', value: 'SAKIET EDDAIER' },
        { gouv: 'SFAX', value: 'MAHRAS' },
        { gouv: 'SFAX', value: 'MENZEL CHAKER' },
        { gouv: 'SFAX', value: 'SFAX EST' },
        { gouv: 'SFAX', value: 'SFAX SUD' },
        { gouv: 'SFAX', value: 'JEBENIANA' },
        { gouv: 'SFAX', value: 'KERKENAH' },
        { gouv: 'SFAX', value: 'SAKIET EZZIT' },

         { gouv: 'SIDI BOUZID', value: 'BEN OUN' },
         { gouv: 'SIDI BOUZID', value: 'BIR EL HAFFEY' },
         { gouv: 'SIDI BOUZID', value: 'JILMA' },
         { gouv: 'SIDI BOUZID', value: 'CEBBALA' },
         { gouv: 'SIDI BOUZID', value: 'OULED HAFFOUZ' },
         { gouv: 'SIDI BOUZID', value: 'MEZZOUNA' },
         { gouv: 'SIDI BOUZID', value: 'REGUEB' },
         { gouv: 'SIDI BOUZID', value: 'SIDI BOUZID OUEST' },
         { gouv: 'SIDI BOUZID', value: 'SOUK JEDID' },
         { gouv: 'SIDI BOUZID', value: 'SIDI BOUZID EST' },
         { gouv: 'SIDI BOUZID', value: 'MAKNASSY' },
         { gouv: 'SIDI BOUZID', value: 'MENZEL BOUZAIENE' },

        { gouv: 'SILIANA', value: 'SILIANA SUD' },
        { gouv: 'SILIANA', value: 'SIDI BOU ROUIS' },
        { gouv: 'SILIANA', value: 'SILIANA NORD' },
        { gouv: 'SILIANA', value: 'GAAFOUR' },
        { gouv: 'SILIANA', value: 'KESRA' },
        { gouv: 'SILIANA', value: 'LE KRIB' },
        { gouv: 'SILIANA', value: 'BOU ARADA' },
        { gouv: 'SILIANA', value: 'BARGOU' },
        { gouv: 'SILIANA', value: 'MAKTHAR' },
        { gouv: 'SILIANA', value: 'ROHIA' },
        { gouv: 'SILIANA', value: 'EL AROUSSA' },

        { gouv: 'SOUSSE', value: 'SIDI BOU ALI' },
        { gouv: 'SOUSSE', value: 'SIDI EL HENI' },
        { gouv: 'SOUSSE', value: 'SOUSSE JAOUHARA' },
        { gouv: 'SOUSSE', value: 'SOUSSE RIADH' },
        { gouv: 'SOUSSE', value: 'SOUSSE VILLE' },
        { gouv: 'SOUSSE', value: 'BOU FICHA' },
        { gouv: 'SOUSSE', value: 'AKOUDA' },
        { gouv: 'SOUSSE', value: 'HAMMAM SOUSSE' },
        { gouv: 'SOUSSE', value: 'KALAA ESSGHIRA' },
        { gouv: 'SOUSSE', value: 'KONDAR' },
        { gouv: 'SOUSSE', value: 'MSAKEN' },
        { gouv: 'SOUSSE', value: 'ENFIDHA' },
        { gouv: 'SOUSSE', value: 'HERGLA' },
        { gouv: 'SOUSSE', value: 'KALAA EL KEBIRA' },
        { gouv: 'SOUSSE', value: 'SOUSSE' },

        { gouv: 'TATAOUINE', value: 'TATAOUINE SUD' },
        { gouv: 'TATAOUINE', value: 'BIR LAHMAR' },
        { gouv: 'TATAOUINE', value: 'DHEHIBA' },
        { gouv: 'TATAOUINE', value: 'GHOMRASSEN' },
        { gouv: 'TATAOUINE', value: 'TATAOUINE NORD' },
        { gouv: 'TATAOUINE', value: 'REMADA' },
        { gouv: 'TATAOUINE', value: 'SMAR' },

         { gouv: 'TOZEUR', value: 'DEGUECHE' },
         { gouv: 'TOZEUR', value: 'TOZEUR' },
         { gouv: 'TOZEUR', value: 'HEZOUA' },
         { gouv: 'TOZEUR', value: 'NEFTA' },
         { gouv: 'TOZEUR', value: 'TAMEGHZA' },

        { gouv: 'TUNIS', value: 'EL MENZAH' },
        { gouv: 'TUNIS', value: 'EL HRAIRIA' },
        { gouv: 'TUNIS', value: 'EL KABBARIA' },
        { gouv: 'TUNIS', value: 'EL KRAM' },
        { gouv: 'TUNIS', value: 'BAB SOUIKA' },
        { gouv: 'TUNIS', value: 'CARTHAGE' },
        { gouv: 'TUNIS', value: 'CITE EL KHADRA' },
        { gouv: 'TUNIS', value: 'BAB BHAR' },
        { gouv: 'TUNIS', value: 'LA MARSA' },
        { gouv: 'TUNIS', value: 'EZZOUHOUR' },
        { gouv: 'TUNIS', value: 'JEBEL JELLOUD' },
        { gouv: 'TUNIS', value: 'SIDI EL BECHIR' },
        { gouv: 'TUNIS', value: 'LA GOULETTE' },
        { gouv: 'TUNIS', value: 'LE BARDO' },
        { gouv: 'TUNIS', value: 'EL OMRANE' },
        { gouv: 'TUNIS', value: 'EL OMRANE SUPERIEUR' },
        { gouv: 'TUNIS', value: 'EL OUERDIA' },
        { gouv: 'TUNIS', value: 'ETTAHRIR' },
        { gouv: 'TUNIS', value: 'SIDI HASSINE' },
        { gouv: 'TUNIS', value: 'ESSIJOUMI' },
        { gouv: 'TUNIS', value: 'LA MEDINA' },

         { gouv: 'ZAGHOUAN', value: 'ENNADHOUR' },
         { gouv: 'ZAGHOUAN', value: 'EL FAHS' },
         { gouv: 'ZAGHOUAN', value: 'BIR MCHERGA' },
         { gouv: 'ZAGHOUAN', value: 'ZAGHOUAN' },
         { gouv: 'ZAGHOUAN', value: 'HAMMAM ZRIBA' },
         { gouv: 'ZAGHOUAN', value: 'SAOUEF' }

    ];



    $scope.filtredDelegations = [];

    $scope.stateInputChanged = function (gouvernerat) {
        $scope.newPraticien.delegation = '';
        $scope.filtredDelegations = [];

        var s = null;
        for (var i = 0; i < $scope.gouvernerats.length - 1; i++) {
            if ($scope.gouvernerats[i] == gouvernerat)
                s = $scope.gouvernerats[i];
        }

        for (var j = 0; j < $scope.delegations.length - 1 ; j++) {
            if ($scope.delegations[j].gouv == s)
                $scope.filtredDelegations.push($scope.delegations[j]);
        }
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

    /** Partie d'ajout d'un patient **/
    $scope.addPraticien = function () {
        $.blockUI();
        $scope.hideValidationError = true;
        praticienApiServices.addPraticien($scope.newPraticien)
            .then(
                // Case : Success.
                function (results) {
                    $.unblockUI();
                    window.scrollTo(0, 0);
                    //Envoyer un email au praticien
                    $scope.contactObject = {
                        from: "contact@allotabib.net",
                        to: $scope.newPraticien.email,
                        sujet: "[AlloTabib] Confirmation d'inscription chez AlloTabib",
                        body: "Bonjour Dr " + $scope.newPraticien.nomPrenom + ",  \n nous vous remercieront pour l’intérêt porté à notre Plateforme AlloTabib., nous confirmons votre inscription, notre équipe qualité vont vérifier vos coordonnées auprès du ministère de santé et ils activeront votre comptre au bout de quelques heures.\n Cordialement \n Equipe AlloTabib."
                    };

                    sendEmail($scope.contactObject);
                    //Envoyé pour moi en alerte
                    $scope.contactObject1 = {
                        from: "contact@allotabib.net",
                        to: "didourebai@gmail.com",
                        sujet: "[AlloTabib] Confirmation d'inscription chez AlloTabib",
                        body: "Bonjour Dr, " + $scope.newPraticien.nomPrenom + " \n nous vous remercieront pour l’intérêt porté à notre Plateforme AlloTabib., nous confirmons votre inscription, notre équipe qualité vont vérifier vos coordonnées auprès du ministère de santé et ils activeront votre comptre au bout de quelques heures.\n Cordialement \n Equipe AlloTabib."
                    };

                    sendEmail($scope.contactObject1);

                    $scope.newPraticien = {};
                    //$location.path('/home');
                    //$scope.sucessMessageTxt = "Votre enregistrement a été effectué avec succès, vous allez reçevoir au bout de 24h un email de confirmation d'inscription.";
                    toastr.success("Votre enregistrement a été effectué avec succès, vous allez reçevoir au bout de 24h un email de confirmation d'inscription.");
                    //Envoyer un email au médecin et à notre compte AlloTabib

                    $scope.EstEninscriptionPatient = false;
                    $scope.isSuccessfullyAdded = true;
                    $scope.addHasErrors = false;
                },
                // Case : Error.
                function (error) {
                    $.unblockUI();
                    //Afficher panel des erreurs
                    $scope.addHasErrors = true;
                    //garder la page d'inscription
                    $scope.EstEninscriptionPatient = true;
                    //ne pas afficher le message de succès
                    $scope.isSuccessfullyAdded = false;

                    // Add the errors messages.

                    if (error.status == 400) {
                        if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                            $scope.addErrorType = "Erreur de validation";
                            $scope.errorMessageTxt = error.data.errors[0].message;
                        }
                    }

                    if (error.status == 401) {
                        $scope.addErrorMessage = JSON.parse(error.data);
                        $scope.errorMessageTxt = "Non autorisé";
                    }

                    if (error.status == 500) {
                        $scope.addErrorType = "Erreur lié au serveur";
                        $scope.errorMessageTxt = error.data.errors[0].exception.Message;
                    } else {
                        $scope.addErrorType = "Erreur anonyme";
                        $scope.errorMessageTxt = error.data.toString();
                    }

                }
            );
    };
    $scope.redirectToHome = function () {
        $location.path('/home');
    };

    $scope.closePanels = function () {
        $('#sucessMessage').slideUp(500);
        $location.path('/home');
    };
});