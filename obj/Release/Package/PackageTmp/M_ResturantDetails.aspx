<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_ResturantDetails.aspx.cs" Inherits="FeelYourFood.M_ResturantDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <link href="js/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="js/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/data-tables/DT_bootstrap.css" />
    
        <link href="js/advanced-datatable/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="jquery.dataTables.min.js"></script>

    <style>
        input.form-control {
            width: 70% !important;
            margin-left: 10px !important;
        }
        th {
    border-bottom: 2px solid #ababab !important;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- page heading start-->
    <div class="page-heading mb-3">
        <h3>Restaurant Details</h3>
    </div>

    <div class="d-flex justify-content-end mb-3">
        <div class="form-check">
            <input class="form-check-input" type="checkbox" id="subscriptionCheckbox" />
            <label class="form-check-label" for="subscriptionCheckbox">Has Subscription?</label>
        </div>
    </div>
    <div class="rest-details">
        <div class="row">
            <div class="table-responsive">
                <table id="dynamic-rest-table" class="display table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Sl.No</th>
                            <th>Restaurant Name</th>
                            <th>Address</th>
                            <th>Phone</th>
                            <th>Email</th>
                            <th>Logo</th>
                            <th>GST No</th>
                            <th>Activation Date</th>
                            <th>Expiry Date</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>

            </div>
        </div>
    </div>
    <script type="text/javascript">
       
        let table = null;

        $(document).ready(function () {
            loadRestaurants();

            $('#subscriptionCheckbox').change(function () {

                loadRestaurants();
            });
        });

        
        function loadRestaurants() {
            const hasSubscription = $('#subscriptionCheckbox').is(':checked');
            console.log("Checkbox is checked:", hasSubscription); // Debug log

            $.ajax({
                type: "POST",
                url: "M_ResturantDetails.aspx/GetRestaurantData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ hasSubscription: hasSubscription }),
                success: function (response) {
                    const data = response.d;
                    const $tbody = $('#dynamic-rest-table tbody');
                    $tbody.empty();

                    if (!data || data.length === 0) {
                        $tbody.append('<tr><td colspan="9" class="text-center">No Restaurant Found.</td></tr>');
                    } else {
                        $.each(data, function (index, item) {
                            $tbody.append(`
                            <tr>
                                <td>${index + 1}</td>
                                <td>${item.ResturantName}</td>
                                <td>${item.RestAddress}</td>
                                <td>${item.RestPhone}</td>
                                <td>${item.EmailId}</td>
                                <td><img src="${item.RestLogo || '/images/NotAvailable.png'}" width="40" height="25" class="rounded" /></td>
                                <td>${item.GstNo}</td>
                                <td>${item.ActivationDate}</td>
                                <td>${item.ExpiryDate}</td>
                            </tr>
                        `);
                        });
                    }
                    if (table) {

                    } else {
                        // Initialize or reinitialize DataTable
                        table = $('#dynamic-rest-table').DataTable({
                            paging: true,
                            searching: true,
                            info: true,
                            destroy: true // ensures safe reinit
                        });
                    }


                },
                error: function (xhr, status, error) {
                    console.error("Error fetching restaurant data:", error);
                    $('#dynamic-rest-table tbody').html('<tr><td colspan="9" class="text-danger text-center">Failed to load data.</td></tr>');
                }
            });
        }
    </script>
    <script>
        function getQueryParam(param) {
            const urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(param);
        }

        function applySearchAfterInit() {
            const nameFromUrl = getQueryParam("name");
            if (nameFromUrl) {
                const decodedName = decodeURIComponent(nameFromUrl);

                const $searchInput = $('input[aria-controls="dynamic-rest-table"]');
                $searchInput.val(decodedName).trigger('input');

                // Simulate Enter key press
                const e = $.Event("keyup", { keyCode: 13 });
                $searchInput.trigger(e);
            }
        }

        $(document).ready(function () {
            setTimeout(applySearchAfterInit, 500);
        });
    </script>





</asp:Content>
