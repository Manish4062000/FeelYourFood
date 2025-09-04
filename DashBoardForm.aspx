<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="DashBoardForm.aspx.cs" Inherits="FeelYourFood.DashBoardForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ordertable td, th {
            text-align: left !important;
            border: 0px !important;
            border-bottom: 1px solid #eeeff1 !important;
            padding: 6px 10px;
        }

        .panel-body {
    padding: 0px !important;
}
        .susbcsiptiondetail {
            text-align: right;
        }
     select#filter {
    float: right;
    border: 1px solid #e9a594;
    padding: 6px;
    width: 160px;
    margin-right: 15px;
    border-radius: 5px;
    background: #fff9f9;
    margin-bottom: 10px;
    color: #000;
}
        .plan-hrd-new strong {
    color: #276b79;
}
.plan-hrd-new h4 {
    font-size: 15px;
    line-height: 23px;
}
.box .title {
    margin-bottom: 2px !important;
}
       table#foodata {
    border: 1px solid #ddd !important;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-heading row">
        <div class="col-md-6">
            <h3>Dashboard</h3>
            <ul class="breadcrumb">
                <li><a href="active">Dashboard</a></li>
            </ul>
        </div>
        <div class="col-md-6">
          
                <div class=" plan-hrd-new susbcsiptiondetail text-end">
                    <h4>Current plan : <strong><span id="subscriptionType" runat="server">Basic</span></strong><br />
                        Expiry On : <strong><span id="expiryDate" runat="server">31st July 2025</span></strong>
                    </h4>
                </div>
           
        </div>
    </div>
    <div>
        <select name="filter" id="filter">
            <option value="today" selected>Today</option>
            <option value="monthly">Monthly</option>
            <option value="yearly">Yearly</option>
        </select>
    </div>
    <div class="col-md-12">
        <!--statistics start-->
        <div class="row state-overviewnew1">
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel red">
                    <div class="symbol">
                        <img src="images/user11.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value">0</div>
                        <div class="title">Orders</div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel blue">
                    <div class="symbol">
                        <img src="images/user22.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value">₹0</div>
                        <div class="title">Sales</div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel green">
                    <div class="symbol">
                        <img src="images/user33.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value">0</div>
                        <div class="title">Total Menu</div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 col-sm-3">
                <div class="panel purple">
                    <div class="symbol">
                        <img src="images/icon44.png" class="img-responsive" alt="">
                    </div>
                    <div class="state-value">
                        <div class="value">0</div>
                        <div class="title">Total Customers</div>
                    </div>
                </div>
            </div>
        </div>
        <!--statistics end-->

        <div class="row">
            <div class="col-md-6">
                <div class="panel">
                    <label>Revenue Chart</label>
                    <div class="panel-body">
                        <canvas id="myLineChart"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel">
                      <label>ConsumeType Chart</label>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <canvas id="pieChart"></canvas>
                            </div>
                            <div class="col-md-6">
                                <ul id="dynamic-legend" class="bar-legend"></ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="panel">
                    <div class="recent-sales box">
                        <div class="title">Kiosk Revenue</div>
                        <div id="page-content-wrapper">
                            <div class="bg-wrapper table-responsive">
                                <table id="restaurantListData" class="data-sestion ">
                                    <thead>
                                        <tr>
                                            <th>Kiosk</th>
                                            <th>Orders</th>
                                            <th>Revenue</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel">
                    <div class="recent-sales box">
                        <div class="title">Mostly Ordered</div>
                        <div id="page-content-wrapper1">
                            <div class="bg-wrapper table-responsive">
                                <table id="foodata" class="data-sestion  ordertable">
                                    <thead>
                                    </thead>
                                    <tbody class="text-left"></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        $(document).ready(function () {
            let pieChartInstance;

            // Call initially for "today"
            fetchDashboardData("today");
            fetchKioskDetails("today");
            loadMostOrderedItems("today");

            // When dropdown value changes
            $("#filter").on("change", function () {
                const filterValue = $(this).val();
                fetchDashboardData(filterValue);
                fetchKioskDetails(filterValue);
                loadMostOrderedItems(filterValue);
            });
            function loadMostOrderedItems(filterType) {
                $.ajax({
                    type: "POST",
                    url: "DashBoardForm.aspx/GetOrderDetails",
                    data: JSON.stringify({ filterType: filterType }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        if (res.d.success) {
                            const rows = res.d.data.map(item => `
                    <tr>
                     <td style="width: 50px !important;"><img src="${item.Photo || 'images/NotAvailable.png'}" style="height: 40px;width:56px"></td>
                        <td>${item.ItemName}<br>₹${item.ItemPrice}</td>
                       
                        <td>${item.TotalQuantity}</td>
                    </tr>
                `).join("");

                            $("#foodata tbody").html(rows);
                        } else {
                            $("#foodata tbody").html("<tr><td colspan='3'>No data found</td></tr>");
                        }
                    },
                    error: function () {
                        $("#foodata tbody").html("<tr><td colspan='3'>Error fetching data</td></tr>");
                    }
                });
            }


            function fetchKioskDetails(filterType) {
                $.ajax({
                    type: "POST",
                    url: "DashBoardForm.aspx/GetKioskDetails",
                    data: JSON.stringify({ filterType: filterType }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d.success) {
                            let kioskData = response.d.data;
                            let html = "";

                            kioskData.forEach(row => {
                                html += `<tr>
                        <td>${row.Kiosk}</td>
                        <td>${row.Orders}</td>
                        <td>₹${row.Revenue}</td>
                    </tr>`;
                            });

                            $("#restaurantListData tbody").html(html);
                        } else {
                            alert("Kiosk Error: " + response.d.message);
                        }
                    },
                    error: function (err) {
                        console.error("Kiosk AJAX Error", err);
                    }
                });
            }


            function fetchDashboardData(filterType) {
                $.ajax({
                    type: "POST",
                    url: "DashBoardForm.aspx/GetRestaurantDetails",
                    data: JSON.stringify({ filterType: filterType }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d.success) {
                            let data = response.d.data;

                            // Update KPIs
                            $(".panel.red .value").text(data.Orders);
                            $(".panel.blue .value").text("₹" + data.Sales);
                            $(".panel.green .value").text(data.Menu);
                            $(".panel.purple .value").text(data.Customers);

                            // Prepare Pie Chart Data
                            let labels = data.ConsumeTypeOrderCounts.map(ct => ct.name);
                            let values = data.ConsumeTypeOrderCounts.map(ct => ct.value);
                            let backgroundColors = ['#115faa', '#ea431f', '#f6d400', '#4BC0C0', '#9966FF']; // Add more colors if needed

                            // Destroy previous chart instance if exists
                            if (pieChartInstance) {
                                pieChartInstance.destroy();
                            }

                            // Draw Pie Chart
                            let ctx = document.getElementById('pieChart').getContext('2d');
                            pieChartInstance = new Chart(ctx, {
                                type: 'doughnut',
                                data: {
                                    labels: labels,
                                    datasets: [{
                                        data: values,
                                        backgroundColor: backgroundColors
                                    }]
                                },
                                options: {
                                    responsive: true,
                                    plugins: {
                                        legend: { display: false }
                                    }
                                }
                            });

                            let legendHtml = '';
                            labels.forEach(function (label, index) {
                                legendHtml += `<li><span style="background-color:${backgroundColors[index]};"></span>${label}</li>`;
                            });
                            $("#dynamic-legend").html(legendHtml);
                            // Revenue Chart
                            let revenueLabels = data.RevenueChart.map(r => r.label);
                            let revenueValues = data.RevenueChart.map(r => r.value);

                            // Destroy previous chart instance if any
                            if (window.revenueChartInstance) {
                                window.revenueChartInstance.destroy();
                            }

                            let ctxRevenue = document.getElementById('myLineChart').getContext('2d');
                            window.revenueChartInstance = new Chart(ctxRevenue, {
                                type: 'line',
                                data: {
                                    labels: revenueLabels,
                                    datasets: [{
                                        label: 'Revenue',
                                        data: revenueValues,
                                        fill: false,
                                        borderColor: '#36A2EB',
                                        backgroundColor: '#36A2EB',
                                        tension: 0.5
                                    }]
                                },
                                options: {
                                    responsive: true,
                                    scales: {
                                        y: {
                                            beginAtZero: true,
                                            title: {
                                                display: true,
                                                text: '₹ Revenue'
                                            }
                                        },
                                        x: {
                                            title: {
                                                display: true,
                                                text: filterType === "today" ? "Hour" : (filterType === "monthly" ? "Day" : "Month")
                                            }
                                        }
                                    },
                                    plugins: {
                                        legend: {
                                            display: true,
                                            position: 'top'
                                        }
                                    }
                                }
                            });

                        } else {
                            alert("Error: " + response.d.message);
                        }
                    },
                    error: function (err) {
                        console.error("AJAX Error", err);
                    }
                });
            }
        });
    </script>
</asp:Content>
