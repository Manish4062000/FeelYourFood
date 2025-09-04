<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_ResturantCrediential.aspx.cs" Inherits="FeelYourFood.M_ResturantCrediential" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .main-check input[type=radio] {
            display: none;
        }

            .main-check input[type=radio]:not(:disabled) ~ label {
                cursor: pointer;
            }

            .main-check input[type=radio]:disabled ~ label {
                color: #bcc2bf;
                border-color: #bcc2bf;
                box-shadow: none;
                cursor: not-allowed;
            }

        .main-check label {
            height: 100%;
            display: block;
            background: white;
            border: 2px solid #cc5e43;
            border-radius: 20px;
            padding: 1rem;
            margin-bottom: 1rem;
            text-align: center;
            box-shadow: 0px 3px 10px -2px rgba(161, 170, 166, 0.5);
            position: relative;
        }

        .main-check input[type=radio]:checked + label {
            background: #e57a60;
            color: white;
            box-shadow: 0px 0px 20px rgb(255 167 145);
        }

            .main-check input[type=radio]:checked + label::after {
                color: #3d3f43;
                font-family: FontAwesome;
                border: 2px solid #e57a60;
                content: "";
                font-size: 24px;
                position: absolute;
                top: -34px;
                left: 50%;
                transform: translateX(-50%);
                height: 50px;
                width: 50px;
                line-height: 50px;
                text-align: center;
                border-radius: 50%;
                background: white;
                box-shadow: 0px 2px 5px -2px rgb(126 25 0);
            }

        .main-check input[type=radio]#control_05:checked + label {
            background: red;
            border-color: red;
        }

        .main-check h2 {
            font-size: 22px;
            margin-bottom: 10px;
            margin-top: 10px;
        }

        .main-check {
            margin-top: 20px;
        }

        .form-group.select-rest {
            margin-top: 14px;
        }

            .form-group.select-rest .form-control {
                border: 1px solid #ffaf9b;
                background: #ffd1c678;
                border-radius: 5px;
            }

        /* date:20-06-2025 sunita */


        .devices-part-1 {
            background: #fff;
            padding: 20px 20px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0.1);
            border-top: 5px solid #266a78;
            border-radius: 10px;
        }


        .responsive-table {
            width: 100%;
            margin-bottom: 1.5em;
            border-spacing: 0;
        }

            .responsive-table thead {
                position: absolute;
                clip: rect(1px 1px 1px 1px);
                /* IE6, IE7 */
                padding: 0;
                border: 0;
                height: 1px;
                width: 1px;
                overflow: hidden;
            }

        @media (min-width: 48em) {
            .responsive-table thead {
                position: relative;
                clip: auto;
                height: auto;
                width: auto;
                overflow: auto;
            }
        }

        .responsive-table thead th {
            background-color: #276b79;
            border: 1px solid #05343e;
            font-weight: normal;
            text-align: center;
            color: white;
        }

            .responsive-table thead th:first-of-type {
                text-align: left;
            }

        .responsive-table tbody,
        .responsive-table tr,
        .responsive-table th,
        .responsive-table td {
            display: block;
            padding: 0;
            text-align: left;
            white-space: normal;
        }

        @media (min-width: 48em) {
            .responsive-table tr {
                display: table-row;
            }
        }

        .responsive-table th,
        .responsive-table td {
            padding: 0.5em;
            vertical-align: middle;
        }

        @media (min-width: 30em) {
            .responsive-table th,
            .responsive-table td {
                padding: 0.75em 0.5em;
            }
        }

        @media (min-width: 48em) {
            .responsive-table th,
            .responsive-table td {
                display: table-cell;
                padding: 0.5em;
            }
        }

        @media (min-width: 62em) {
            .responsive-table th,
            .responsive-table td {
                padding: 0.75em 0.5em;
            }
        }

        @media (min-width: 75em) {
            .responsive-table th,
            .responsive-table td {
                padding: 0.75em;
            }
        }

        .responsive-table caption {
            margin-bottom: 1em;
            font-size: 1em;
            font-weight: bold;
            text-align: center;
        }

        @media (min-width: 48em) {
            .responsive-table caption {
                font-size: 1.5em;
            }
        }

        .responsive-table tfoot {
            font-size: 0.8em;
            font-style: italic;
        }

        @media (min-width: 62em) {
            .responsive-table tfoot {
                font-size: 0.9em;
            }
        }

        @media (min-width: 48em) {
            .responsive-table tbody {
                display: table-row-group;
            }
        }

        .responsive-table tbody tr {
            margin-bottom: 1em;
        }

        @media (min-width: 48em) {
            .responsive-table tbody tr {
                display: table-row;
                border-width: 1px;
            }
        }

        .responsive-table tbody tr:last-of-type {
            margin-bottom: 0;
        }

        @media (min-width: 48em) {
            .responsive-table tbody tr:nth-of-type(even) {
                background-color: rgb(5 52 62 / 8%);
            }
        }

        .responsive-table tbody th[scope=row] {
            background-color: #26890d;
            color: white;
        }

        @media (min-width: 30em) {
            .responsive-table tbody th[scope=row] {
                border-left: 1px solid #05343e;
                border-bottom: 1px solid #05343e;
            }
        }

        @media (min-width: 48em) {
            .responsive-table tbody th[scope=row] {
                background-color: transparent;
                color: #000001;
                text-align: left;
            }
        }

        .responsive-table tbody td {
            text-align: right;
        }

        @media (min-width: 48em) {
            .responsive-table tbody td {
                border-left: 1px solid #05343e;
                border-bottom: 1px solid #05343e;
                text-align: center;
            }
        }

        @media (min-width: 48em) {
            .responsive-table tbody td:last-of-type {
                border-right: 1px solid #05343e;
                vertical-align: baseline;
            }
        }

        .responsive-table tbody td[data-type=currency] {
            text-align: right;
        }

        .responsive-table tbody td[data-title]:before {
            content: attr(data-title);
            float: left;
            font-size: 0.8em;
            color: rgba(0, 0, 0, 0.54);
        }

        @media (min-width: 30em) {
            .responsive-table tbody td[data-title]:before {
                font-size: 0.9em;
            }
        }

        @media (min-width: 48em) {
            .responsive-table tbody td[data-title]:before {
                content: none;
            }
        }

        .devices-part h2 {
            margin-top: 0;
            font-size: 25px;
            margin-bottom: 20px;
        }

        table.menu-table tr th {
            background: #e5ffdbd4;
            color: #000;
            border: 1px solid #a0a4a0;
            text-align: center !important;
            font-weight: 600;
        }

        .menu-table tbody td {
            border-left: 1px solid #a0a4a0;
            border-bottom: 1px solid #a0a4a0;
            text-align: center;
        }

            .menu-table tbody td:last-of-type {
                border-right: 1px solid #a0a4a0;
            }

        table.menu-non-table tr th {
            background: #f8d4d4;
            border: 1px solid #a0a4a0;
            color: #000;
            text-align: center !important;
            font-weight: 600;
        }

        .menu-non-table tbody td {
            border-left: 1px solid #a0a4a0;
            border-bottom: 1px solid #a0a4a0;
            text-align: center;
        }

            .menu-non-table tbody td:last-of-type {
                border-right: 1px solid #a0a4a0;
            }

        .toggle-label {
            position: relative;
            display: block;
            width: 260px;
            height: 40px;
            border: 1px solid #fff;
            float: right;
        }

            .toggle-label input[type=checkbox] {
                opacity: 0;
                position: absolute;
                width: 100%;
                height: 100%;
            }

                .toggle-label input[type=checkbox] + .back {
                    position: absolute;
                    width: 100%;
                    height: 100%;
                    background: #ed1c24;
                    transition: background 150ms linear;
                }

                .toggle-label input[type=checkbox]:checked + .back {
                    background: #00a651; /*green*/
                }

                .toggle-label input[type=checkbox] + .back .toggle {
                    display: block;
                    position: absolute;
                    content: ' ';
                    background: #fff;
                    width: 50%;
                    height: 100%;
                    transition: margin 150ms linear;
                    border: 1px solid #a3a3a3;
                    border-radius: 0;
                }

        .btn:focus, .btn:active:focus, .btn.active:focus {
            /* outline: thin dotted; */
            outline: none;
            outline-offset: -2px;
        }

        .toggle-label input[type=checkbox]:checked + .back .toggle {
            margin-left: 130px;
        }

        .toggle-label .label {
            display: block;
            position: absolute;
            width: 50%;
            color: #ddd;
            line-height: 30px;
            text-align: center;
            font-size: 15px;
        }

            .toggle-label .label.on {
                left: 0px;
            }

            .toggle-label .label.off {
                right: 0px;
            }

        .toggle-label input[type=checkbox]:checked + .back .label.on {
            color: #fff;
        }

        .toggle-label input[type=checkbox] + .back .label.off {
            color: #fff;
        }

        .toggle-label input[type=checkbox]:checked + .back .label.off {
            color: #ddd;
        }

        label.toggle-label img {
            width: 25px;
        }

        .foodmenu button.btn.btn-sm.btn-primary:hover {
            box-shadow: -5px 3px 0px -1px #a35745;
            transition: all linear 0.5s;
        }

        .foodmenu button.btn.btn-sm.btn-primary {
            background: #ff714e;
            padding: 3px 20px;
            border: none;
            box-shadow: 4px 3px 0px -1px #a35745;
        }

        #style-3::-webkit-scrollbar {
            width: 6px;
            background-color: #F5F5F5;
        }

        #style-3::-webkit-scrollbar-thumb {
            background-color: #dcdce0;
        }

        .scrollbar {
            height: 185px;
            width: auto;
            overflow-y: overlay;
            /*margin-bottom: 10px;*/
        }

        .force-overflow {
            min-height: 70px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-heading">
        <h3>Restaurant Credientials</h3>
    </div>
    <!-- page heading end-->
    <div class="rest-details">
        <div class="row">
            <div class="col-md-2"></div>

            <div class="col-md-8">
                <!-- All -->
                <div class="col-md-4">
                    <div class="main-check">
                        <input type="radio" id="control_01" name="select" value="1">
                        <label for="control_01">
                            <h2>All</h2>
                        </label>
                    </div>
                </div>

                <!-- Subscription -->
                <div class="col-md-4">
                    <div class="main-check">
                        <input type="radio" id="control_02" name="select" value="2" checked>
                        <label for="control_02">
                            <h2>Subscription</h2>
                        </label>
                    </div>
                </div>

                <!-- No Subscription -->
                <div class="col-md-4">
                    <div class="main-check">
                        <input type="radio" id="control_03" name="select" value="3">
                        <label for="control_03">
                            <h2>No Subscription</h2>
                        </label>
                    </div>
                </div>

                <!-- Dropdown -->
                <div class="col-md-12">
                    <div class="form-group select-rest">
                        <select id="ddlData" class="default-select wide form-control ms-0"></select>
                    </div>
                </div>
            </div>

            <div class="col-md-2"></div>


        </div>
    </div>

    <div class="row">
        <!-- Devices Section -->
        <div id="devicetab" class="col-md-5" style="display: none;">
            <div class="devices-part-1">
                <div class="devices-part">
                    <h2>Devices</h2>
                    <table class="responsive-table">
                        <thead>
                            <tr>
                                <th><b>Devices Name</b></th>
                                <th><b>Quantity</b></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row">Local Server</th>
                                <td id="lblServer" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Ordering Kiosk</th>
                                <td id="lblOrderingKiosk" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Kitchen Display</th>
                                <td id="lblKitchenDisplay" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">QMS Screens</th>
                                <td id="lblQMS" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Table Tablet</th>
                                <td id="lblTableTab" data-title="Released">N/A</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Configuration Section -->
        <div id="Configurationtab" class="col-md-7" style="display: none;">
            <div class="devices-part-1">
                <div class="devices-part">
                    <h2>Configuration</h2>
                    <table class="responsive-table">
                        <thead>
                            <tr>
                                <th><b>Type</b></th>
                                <th><b>Details</b></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row">Cuisines</th>
                                <td id="cuisine" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Consume Type</th>
                                <td id="consume" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Categories</th>
                                <td id="category" data-title="Released">N/A</td>
                            </tr>
                            <tr>
                                <th scope="row">Payment Mode</th>
                                <td id="payment" data-title="Released">N/A</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <br />
    <div class="row">
        <div id="menutab" class="col-md-12" style="display: none;">
            <div class="devices-part-1">
                <div class="devices-part">
                    <div class="row">
                        <div class="col-md-6">
                            <h2>Menu Details</h2>
                        </div>
                        <div class="col-md-6">
                            <label class='toggle-label'>
                                <input type='checkbox' id="vegToggle" />
                                <span class='back'>
                                    <span class='toggle'></span>
                                    <span class='label on'>
                                        <img src="image/foodcategory/Manish_20022025124438veg.jpg" />
                                        Veg
                                    </span>
                                    <span class='label off'>
                                        <img src="image/foodcategory/Manish_20022025124607NonVeg.jpg" />
                                        Non Veg
                                    </span>
                                </span>
                            </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">

                            <h4 id="menuHeading" style="text-align: center; background: #009900; border: 1px solid #a0a4a0; padding: 0.50em; color: #fff; margin-top: 0; margin-bottom: 0;">
                                <b>Veg</b>
                            </h4>
                            <div class="scrollbar" id="style-5">
                                <div class="force-overflow">
                                    <table class="menu-table foodmenu" border="1" style="width: 100%; text-align: center;">
                                        <thead>
                                            <tr>
                                                <th style="padding: 0.30em; width: 40%;">Menu</th>
                                                <th style="padding: 0.30em; width: 30%;">Photo</th>
                                                <th style="padding: 0.30em; width: 30%;">Item Details</th>
                                            </tr>
                                        </thead>

                                        <tbody>
                                            <!-- Dynamic rows -->
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id="itemTable" class="col-md-6 itemTable" style="display: none;">
                            <h4 id="itemheading" style="text-align: center; background: #009900; border: 1px solid #a0a4a0; padding: 0.50em; color: #fff; margin-top: 0; margin-bottom: 0;">
                                <b>Item Details</b>
                            </h4>
                            <div class="scrollbar" id="style-4">
                                <div class="force-overflow">
                                    <table class="menu-table" border="1" style="width: 100%; text-align: center;">
                                        <thead>
                                            <tr>
                                                <th style="padding: 0.30em; width: 60%;">Food Item</th>
                                                <th style="padding: 0.30em; width: 40%;">Photo</th>
                                            </tr>
                                        </thead>

                                        <tbody>
                                            <!-- Dynamic rows -->
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>




                </div>
            </div>
        </div>

    </div>
    <br />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[name="select"]').change(function () {
                var selectedValue = $('input[name="select"]:checked').val();
                $('#devicetab').hide();
                $('#Configurationtab').hide();
                $('#menutab').hide();
                $('#itemTable').hide();
                $.ajax({
                    type: "POST",
                    url: "M_ResturantCrediential.aspx/GetFilteredData",
                    data: JSON.stringify({ selectedOption: selectedValue }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var ddl = $('#ddlData');
                        ddl.empty();
                        ddl.append($('<option>').val('').text('-- Select Restaurant --'));

                        $.each(response.d, function (i, item) {
                            ddl.append($('<option></option>').val(item.Id).html(item.Name));
                        });
                    },
                    error: function (err) {
                        console.error("AJAX Error:", err);
                        alert("Error: " + err.responseText);
                    }
                });
            });

            $('input[name="select"]').change();
        });

        let currentRestId = null;

        $('#ddlData').on('change', function () {
            var selectedId = $(this).val();
            if (selectedId) {
                currentRestId = selectedId;

                $.ajax({
                    type: "POST",
                    url: "M_ResturantCrediential.aspx/GetRestaurantDetails",
                    data: JSON.stringify({ restId: selectedId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var data = response.d;

                        $('#cuisine').text(data.Cuisine);
                        $('#consume').text(data.Consume);
                        $('#category').text(data.Category);
                        $('#payment').text(data.Payment);
                        $('#lblServer').text(data.Server);
                        $('#lblOrderingKiosk').text(data.OrderingKiosk);
                        $('#lblKitchenDisplay').text(data.KitchenDisplay);
                        $('#lblQMS').text(data.QMS);
                        $('#lblTableTab').text(data.TableTab);

                        $('#devicetab, #Configurationtab, #menutab, #Itemtab').show();

                        // Show the correct menu based on toggle state
                        loadMenuBasedOnToggle();
                    },
                    error: function (err) {
                        console.error("Error:", err);
                    }
                });
            }
        });
        $(document).ready(function () {
            $('#vegToggle').prop('checked', true); 
            $('#itemTable').hide();
        });

        $('#vegToggle').on('change', function () {
            if (currentRestId) {
                loadMenuBasedOnToggle();
                $('#itemTable').hide();
            }
        });

        function loadMenuBasedOnToggle() {
            const isVeg = $('#vegToggle').is(':checked');
            if (isVeg) {
                updateMenuHeading("Veg");
                loadMenuDetails(currentRestId);
            } else {

                updateMenuHeading("Non Veg");
                loadnonMenuDetails(currentRestId);
            }
        }

        // Update heading text and color
        function updateMenuHeading(text) {
            let color = text === "Veg" ? "#009900" : "#cc0000";
            $('#menuHeading')
                .css("background", color)
                .html("<b>" + text + "</b>");
            $('#itemheading')
                .css("background", color)
                
        }

        // Veg menu loader
        function loadMenuDetails(restId) {
            $.ajax({
                type: "POST",
                url: "M_ResturantCrediential.aspx/GetVegMenuDetails",
                data: JSON.stringify({ restId: restId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    let data = response.d;
                    let rows = data.length === 0
                        ? "<tr><td colspan='3'>No menu found</td></tr>"
                        : data.map(item =>
                            "<tr>" +
                            "<td style='width: 40%;'>" + item.FoodMenuName + "</td>" +
                            "<td style='width: 30%;'><img src='" + item.MenuPhotoPath + "' alt='Menu Photo' style='width: 100px; height: 50px;'/></td>" +
                            "<td style='width: 30%;'><button type='button' class='btn btn-sm btn-primary' onclick='viewItemDetails(" + restId + ", " + item.FoodMenuId + ", \"Veg\")'>View</button></td>"
                        ).join('');


                    $(".foodmenu tbody").html(rows);
                },
                error: function (err) {
                    console.error("Veg Menu load error:", err);
                }
            });
        }

        // Non-veg menu loader
        function loadnonMenuDetails(restId) {
            $.ajax({
                type: "POST",
                url: "M_ResturantCrediential.aspx/GetNonVegMenuDetails",
                data: JSON.stringify({ restId: restId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    let data = response.d;
                    let rows = data.length === 0
                        ? "<tr><td colspan='3'>No menu found</td></tr>"
                        : data.map(item =>
                            "<tr>" +
                            "<td style='width: 40%;'>" + item.FoodMenuName + "</td>" +
                            "<td style='width: 30%;'><img src='" + item.MenuPhotoPath + "' alt='Menu Photo' style='width: 100px; height: 50px;'/></td>" +
                            "<td style='width: 30%;'><button type='button' class='btn btn-sm btn-primary' onclick='viewItemDetails(" + restId + ", " + item.FoodMenuId + ", \"Non Veg\")'>View</button></td></tr>"
                        ).join('');


                    $(".foodmenu tbody").html(rows);
                },
                error: function (err) {
                    console.error("Non Veg Menu load error:", err);
                }
            });
        }
        //Itemdetails
        function viewItemDetails(restId, foodMenuId, categoryType) {
            $.ajax({
                type: "POST",
                url: "M_ResturantCrediential.aspx/GetItemDetails",
                data: JSON.stringify({ restId: restId, foodMenuId: foodMenuId, categoryType: categoryType }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const data = response.d;

                    let rows = data.length === 0
                        ? "<tr><td colspan='2'>No items found</td></tr>"
                        : data.map(item =>
                            "<tr>" +
                            "<td style='width: 60%;'>" + item.ItemName + "</td>" +
                            "<td style='width: 40%;'><img src='" + item.ItemPhotoPath + "' alt='Item Photo' style='width: 100px; height: 50px;'/></td>" +
                            "</tr>"
                        ).join('');

                    $(".itemTable tbody").html(rows);
                    $(".itemTable").show();
                },
                error: function (err) {
                    console.error("Item details load error:", err);
                }
            });
        }




    </script>



</asp:Content>
