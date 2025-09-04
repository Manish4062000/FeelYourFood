<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_Dashboard.aspx.cs" Inherits="FeelYourFood.M_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

    <div class="page-heading">
        <h3>Dashboard
    </h3>
        <ul class="breadcrumb">
            <li>
                <a href="active">Dashboard</a>
            </li>
           
        </ul>

    </div>
    <div class="col-md-12">
        <!--statistics start-->
        <div class="row state-overview">
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel purple">
                    <div class="symbol">
                        <img src="images/icon1.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value"></div>
                        <div class="title">Total Registration</div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel red">
                    <div class="symbol">
                        <img src="images/icon2.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value"></div>
                        <div class="title">Total Subscription  </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel blue">
                    <div class="symbol">
                        <img src="images/icon3.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value"></div>
                        <div class="title">
                            No
                            <br>
                            Subscrption
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel green">
                    <div class="symbol">
                        <img src="images/icon4.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value"></div>
                        <div class="title">Active Subscription</div>
                    </div>
                </div>
            </div>
        </div>
        <!--statistics end-->
        <div class="row">
            <div class="col-md-6">
                <div class="panel">
                    <div class="panel-body">
                        <canvas id="myLineChart"></canvas>
                    </div>

                </div>
            </div>

            <div class="col-md-6">
                <div class="panel">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div id="graph-donut" class="revenue-graph"></div>

                            </div>
                            <div class="col-md-6">
                                <ul class="bar-legend">
                                    <li><span class="green"></span>Ordering Kiosk </li>
                                    <li><span class="blue"></span>QMS </li>
                                    <li><span class="yellow"></span>kitchen Display </li>
                                    <li><span class="red"></span>Server </li>
                                    <li><span class="purple"></span>Table Tablet </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <div class="recent-sales box">
                        <div class="title">Recent Registered Restaurant</div>
                        <hr style="margin-top: 10px;margin-bottom: 10px;"/>
                        <div id="page-content-wrapper">
                            <div class="bg-wrapper table-responsive">
                                <table id="restaurantListData" class="data-sestion">
                                    <thead>
                                        <tr>
                                            <th>Sl No</th>
                                            <th>Restaurant Name</th>
                                            <th>Address</th>
                                            <th>Phone</th>
                                            <th>Email</th>
                                            <th>Logo</th>
                                            <th>More Details</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>


                            </div>
                        </div>

                    </div>
                </div>
            </div>


        </div>
    </div>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "M_Dashboard.aspx/GetRestaurantList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const data = response.d;
                    let html = "";

                    if (data.length > 0) {
                        $.each(data, function (index, item) {
                            html += `<tr>
                    <td>${index + 1}</td>
                    <td>${item.Name}</td>
                    <td>${item.Address}</td>
                    <td>${item.Phone}</td>
                    <td>${item.Email}</td>
                    <td><img src="${item.Logo || '/images/NotAvailable.png'}" width="40" height="25" class="rounded" /></td>
                    <td><a href="M_ResturantDetails.aspx?name=${encodeURIComponent(item.Name)}" class="btn btn-sm btn-info">More</a></td>

                </tr>`;
                        });
                    } else {
                        html = `<tr><td colspan="6" class="text-center">No data found</td></tr>`;
                    }

                    $("#restaurantListData tbody").html(html);
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", error);
                    console.error("Response Text:", xhr.responseText);
                    $("#restaurantListData tbody").html("<tr><td colspan='6' class='text-danger text-center'>Error loading data</td></tr>");
                }
            });

        });
    </script>
    <script>
        $(function () {
            loadDashboardCounts();

            setInterval(loadDashboardCounts, 5000);
        });

        function loadDashboardCounts() {
            $.ajax({
                type: "POST",
                url: "M_Dashboard.aspx/GetRestaurants",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var d = msg.d;
                    $(".panel.purple .state-value .value").text(d.Total);
                    $(".panel.red .state-value .value").text(d.Approved);
                    $(".panel.blue .state-value .value").text(d.NotApproved);
                    $(".panel.green .state-value .value").text(d.ActiveSubs);
                },
                error: function (xhr, status, err) {
                    console.error("Error fetching dashboard counts:", err);
                }
            });
        }
    </script>

    <script src="js/morris-chart/morris.js"></script>
    <script src="js/morris-chart/raphael-min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="js/dashboard-chart-init.js"></script>
    <script>
        let myLineChartInstance = null;

        function renderTwoLineChart() {
            $.ajax({
                type: "POST",
                url: "M_Dashboard.aspx/SubscriptionData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const data = response.d;

                    const currentMonth = new Date().getMonth(); // Jan = 0, Jun = 5, etc.

                    // Trim the data to include only current or past months
                    const labels = data.Labels.slice(0, currentMonth + 1);
                    const registrations = data.Registrations.slice(0, currentMonth + 1);
                    const subscriptions = data.Subscriptions.slice(0, currentMonth + 1);

                    const ctx = document.getElementById('myLineChart').getContext('2d');

                    // Destroy previous chart to avoid overlapping
                    if (myLineChartInstance) {
                        myLineChartInstance.destroy();
                    }

                    // Create chart
                    myLineChartInstance = new Chart(ctx, {
                        type: 'line',
                        data: {
                            labels: labels,
                            datasets: [
                                {
                                    label: 'Total Registration',
                                    data: registrations,
                                    borderColor: '#3799e1',
                                    backgroundColor: 'transparent',
                                    tension: 0.4
                                },
                                {
                                    label: 'Total Subscription',
                                    data: subscriptions,
                                    borderColor: '#4bb905',
                                    backgroundColor: 'transparent',
                                    tension: 0.4
                                }
                            ]
                        },
                        options: {
                            responsive: true,
                            animation: false,
                            plugins: {
                                legend: { display: true }
                            },
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    });
                },
                error: function (err) {
                    console.error("Error loading subscription chart:", err);
                }
            });
        }

        $(function () {
            renderTwoLineChart();
            setInterval(renderTwoLineChart, 10000); 
        });
    </script>

    <script>
        function loadDeviceDonutChart() {
            $.ajax({
                type: "POST",
                url: "M_Dashboard.aspx/DeviceData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;

                    if (data.length > 0) {
                        var device = data[0];
                        var donutData = [
                            { label: "Ordering Kiosk", value: parseInt(device.OrderingKiosk) || 0 },
                            { label: "Server", value: parseInt(device.ServerCount) || 0 },
                            { label: "kitchen Display", value: parseInt(device.kitchendisplay) || 0 },
                            { label: "QMS", value: parseInt(device.qms) || 0 },
                            { label: "Table Tablet", value: parseInt(device.tabletab) || 0 }
                        ];

                        $('#graph-donut').empty();

                        Morris.Donut({
                            element: 'graph-donut',
                            data: donutData,
                            colors: ['#2ecc71', '#e74c3c', '#f1c40f', '#3498db', '#9b59b6'],
                            resize: true
                        });
                    }
                },
                error: function (err) {
                    console.error("Error loading device data:", err);
                }
            });
        }

        $(function () {
            loadDeviceDonutChart();

            setInterval(loadDeviceDonutChart, 500);
        });
    </script>




</asp:Content>
