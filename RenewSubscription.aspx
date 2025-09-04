<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="RenewSubscription.aspx.cs" Inherits="FeelYourFood.RenewSubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sub-table tbody td {
            text-align: center;
        }
        .table {
    margin-bottom: 0px !important;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-heading">
        <h3>Subscription Details
            </h3>
    </div>
    <div class="rest-details">
        <asp:HiddenField ID="hfRestId" runat="server" />

        <div class="row">
            <div class="col-md-12 sub-msg" align="center">
                <div class="plan-hrd">
                    <h4>Your current plan is 
        <strong><span id="subscriptionType" runat="server">Basic</span></strong>
                        and it will expire on 
        <strong><span id="expiryDate" runat="server">31st July 2025</span></strong>.
                   
                    </h4>
                    <h5>To ensure uninterrupted service, please renew your subscription before the expiry date.</h5>
                </div>
            </div>


            <div class="col-md-12 sub-msg" align="center">
                <br />
            </div>
            <section class="pricing-section">

                <div class="outer-box">

                    <div class="row">
                        <!-- Pricing Block -->
                        <div class="pricing-block col-lg-4 col-md-6 col-sm-12 wow fadeInUp">
                            <div class="inner-box active" id="planQuarterly" data-multiplier="1" data-emitype="Quarterly">
                                <div class="icon-box">
                                    <div class="icon-outer"><i class="fa fa-location-arrow"></i></div>
                                </div>
                                <div class="price-box">
                                    <div class="title">Quarterly </div>
                                    <h4 class="price"><span id="totalQuarterly">0</span></h4>
                                </div>
                                <ul class="features">
                                    <li class="true">Subscription Price <span id="subPriceQuarterly">0</span></li>
                                </ul>
                                <div class="btn-box">
                                    <a href="#" class="theme-btn">BUY plan</a>
                                </div>
                            </div>
                        </div>

                        <!-- Pricing Block -->
                        <div class="pricing-block col-lg-4 col-md-6 col-sm-12 wow fadeInUp" data-wow-delay="400ms">
                            <div class="inner-box" id="planHalfYearly" data-multiplier="2" data-emitype="HalfYearly">
                                <div class="icon-box">
                                    <div class="icon-outer"><i class="fa fa-gift"></i></div>
                                </div>
                                <div class="price-box">
                                    <div class="title">Half yearly</div>
                                    <h4 class="price"><span id="totalHalfYearly">0</span></h4>
                                </div>
                                <ul class="features">
                                    <li class="true">Subscription Price <span id="subPriceHalfYearly">0</span></li>
                                </ul>
                                <div class="btn-box">
                                    <a href="#" class="theme-btn">BUY plan</a>
                                </div>
                            </div>
                        </div>

                        <!-- Pricing Block -->
                        <div class="pricing-block col-lg-4 col-md-6 col-sm-12 wow fadeInUp" data-wow-delay="800ms">
                            <div class="inner-box" id="planYearly" data-multiplier="4" data-emitype="Yearly">
                                <div class="icon-box">
                                    <div class="icon-outer"><i class="fa fa-rocket"></i></div>
                                </div>
                                <div class="price-box">
                                    <div class="title">Yearly</div>
                                    <h4 class="price"><span id="totalYearly">0</span></h4>
                                </div>
                                <ul class="features">
                                    <li class="true">Subscription Price <span id="subPriceYearly">0</span></li>
                                </ul>
                                <div class="btn-box">
                                    <a href="#" class="theme-btn">BUY plan</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <asp:HiddenField ID="hfSelectedPlan" runat="server" />

    <div class="rest-details-sub-main">
        <section id="detailTable">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal sub-table">
                        <table class="table table-bordered text-center">
                            <thead>
                                <tr>
                                    <th>Sl.No</th>
                                    <th>Device</th>
                                    <th>Subscription Price (Per piece)</th>
                                    <th>Quantity</th>
                                    <th>Total Subscription</th>
                                </tr>
                            </thead>
                            <tbody id="deviceTableBody"></tbody>
                        </table>

                    </div>

                    <div class="form-horizontal sub-table-2">
                        <span style="font-size: large; font-weight: 700;">Billing</span>
                        <table class="table table-bordered text-center">
                            <thead>
                                <tr>
                                    <th>Sl.No</th>
                                    <th>Items</th>
                                    <th>Price</th>
                                    <th>Discount</th>
                                    <th>Taxable Amount</th>
                                    <th>GST (%)</th>
                                    <th>GST Amount</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody id="billingTableBody"></tbody>
                        </table>
                    </div>

                    <div class="row">
                        <div class="col-md-9"></div>
                        <div class="col-md-3">
                            <div class="price-list">
                                <label>Total Amount: <span id="finalgrandtotal">₹ 0.00</span></label>
                                <label>Roundoff: <span id="roundoff">₹ 0.00</span></label>
                                <label>Payable Amount: <span id="payableamt">₹ 0.00</span></label>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-md-12" align="center">
                    <asp:Button ID="Button" runat="server" CssClass="theme-btn" Text="Proceed" OnClientClick="return handleProceedClick();" />

                </div>
            </div>



        </section>
    </div>

    <div class="panel" style="margin-top: 10px;">

        <div class="accordion-group" id="customAccordion">

            <div class="sub-emi">
                <div class="accordion-header collapsed" data-toggle="collapse" data-target="#item1">
                    Transaction History
           
                <span class="glyphicon glyphicon-chevron-down"></span>
                </div>
                <div id="item1" class="accordion-body collapse">
                    <div class="recent-sales box">

                        <div id="page-content-wrapper">
                            <div class="bg-wrapper table-responsive">
                                <table class="table table-bordered text-center">
                                    <thead >
                                        <tr>
                                            <th>Sl No</th>
                                            <th>Payment Type</th>
                                            <th>Payable Amount</th>
                                            <th>Payment Done</th>
                                            <th>Last Payment</th>
                                            <th>Activation Date</th>
                                            <th>Expiry Date</th>
                                        </tr>
                                    </thead>
                                    <tbody id="paymentTableBody" runat="server"></tbody>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="emiModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document" style="max-width: 700px;">
            <div class="modal-content">
                <div class="modal-body" style="padding: 0;">
                    <iframe id="emiFrame" src="" width="100%" height="500px" style="border: none;"></iframe>
                </div>
            </div>
        </div>
    </div>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        function formatPrice(value) {
            return new Intl.NumberFormat('en-IN', {
                style: 'currency',
                currency: 'INR',
                minimumFractionDigits: 2
            }).format(value || 0);
        }

        function loadSubscriptionData(renderTable = true) {
            const restId = document.getElementById("<%= hfRestId.ClientID %>").value;

            const plans = {
                "Quarterly": { multiplier: 1 },
                "HalfYearly": { multiplier: 2 },
                "Yearly": { multiplier: 4 }
            };

            $.ajax({
                type: "POST",
                url: "RenewSubscription.aspx/showdevicedata",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ restid: restId }),
                dataType: "json",
                success: function (response) {
                    const data = response.d;
                    const tbody = document.getElementById("deviceTableBody");
                    if (renderTable && tbody) tbody.innerHTML = "";

                    const totals = {
                        Quarterly: { sub: 0 },
                        HalfYearly: { sub: 0 },
                        Yearly: { sub: 0 }
                    };

                    data.forEach((item, index) => {
                        const baseSub = parseFloat(item.BaseSubscriptionRate || item.SubscriptionPrice || 0);
                        const qty = parseInt(item.Quantity || 0);

                        totals.Quarterly.sub += baseSub * qty * plans.Quarterly.multiplier;
                        totals.HalfYearly.sub += baseSub * qty * plans.HalfYearly.multiplier;
                        totals.Yearly.sub += baseSub * qty * plans.Yearly.multiplier;

                        if (renderTable && tbody) {
                            const row = `
                            <tr>
                                <td>${index + 1}</td>
                                <td>${item.DeviceName}</td>
                                <td>${formatPrice(baseSub)}</td>
                                <td>${qty}</td>
                                <td>${formatPrice(baseSub * qty)}</td>
                            </tr>`;
                            tbody.insertAdjacentHTML("beforeend", row);
                        }
                    });

                    // Update plan prices on cards
                    document.getElementById("subPriceQuarterly").innerText = formatPrice(totals.Quarterly.sub);
                    document.getElementById("subPriceHalfYearly").innerText = formatPrice(totals.HalfYearly.sub);
                    document.getElementById("subPriceYearly").innerText = formatPrice(totals.Yearly.sub);

                    document.getElementById("totalQuarterly").innerText = formatPrice(totals.Quarterly.sub);
                    document.getElementById("totalHalfYearly").innerText = formatPrice(totals.HalfYearly.sub);
                    document.getElementById("totalYearly").innerText = formatPrice(totals.Yearly.sub);

                    if (renderTable) {
                        document.getElementById("detailTable").style.display = "block";
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error loading subscription data:", error);
                }
            });
        }

        function loadDeviceAndBillingTables() {
            const restId = document.getElementById("<%= hfRestId.ClientID %>").value;

            const plans = {
                "Quarterly": 1,
                "HalfYearly": 2,
                "Yearly": 4
            };

            let selectedPlan = "Quarterly"; // default
            let emiType = "Quarterly"; // default

            const activeBox = document.querySelector(".pricing-block .inner-box.active");
            if (activeBox) {
                const id = activeBox.id.replace("plan", "");
                if (plans[id]) {
                    selectedPlan = id;
                }
                emiType = activeBox.getAttribute("data-emitype") || selectedPlan;
            }

            const multiplier = plans[selectedPlan];

            $.ajax({
                type: "POST",
                url: "RenewSubscription.aspx/showdevicedata",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ restid: restId }),
                dataType: "json",
                success: function (response) {
                    const data = response.d;
                    const billingBody = document.getElementById("billingTableBody");
                    billingBody.innerHTML = "";

                    let baseSubscriptionTotal = 0;
                    let subscriptionDiscount = 0;
                    let softwareGSTPercent = 0;

                    data.forEach((item, index) => {
                        if (index === 0) {
                            subscriptionDiscount = parseFloat(item.SubscriptionDiscount || 0);
                            softwareGSTPercent = parseFloat(item.SoftwareGST || 0);
                        }

                        const baseSub = parseFloat(item.BaseSubscriptionRate || item.SubscriptionPrice || 0);
                        const qty = parseInt(item.Quantity || 0);
                        baseSubscriptionTotal += (baseSub * qty);
                    });

                    const multipliedSubscriptionTotal = baseSubscriptionTotal * multiplier;
                    const discountAmount = (multipliedSubscriptionTotal * subscriptionDiscount) / 100;
                    const taxable = multipliedSubscriptionTotal - discountAmount;
                    const gstAmount = (taxable * softwareGSTPercent) / 100;
                    const grandTotal = taxable + gstAmount;
                    const rounded = Math.round(grandTotal);
                    const roundoff = rounded - grandTotal;

                    const rowHtml = `
                <tr>
                    <td>1</td>
                    <td>Subscription Charges (${selectedPlan})</td>
                    <td>${formatPrice(multipliedSubscriptionTotal)}</td>
                    <td>${subscriptionDiscount}%</td>
                    <td>${formatPrice(taxable)}</td>
                    <td>${softwareGSTPercent}%</td>
                    <td>${formatPrice(gstAmount)}</td>
                    <td>${formatPrice(grandTotal)}</td>
                </tr>`;
                    billingBody.insertAdjacentHTML("beforeend", rowHtml);

                    // ✅ Pass both amount and emiType to server
                    $.ajax({
                        type: "POST",
                        url: "RenewSubscription.aspx/StoreDueAmount",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            amount: rounded,
                            emiType: emiType
                        }),
                        dataType: "json",
                        success: function () {
                            console.log("DueAmount and emiType stored in session:", rounded, emiType);
                        },
                        error: function (xhr, status, error) {
                            console.error("Failed to store due amount:", error);
                        }
                    });

                    // Update summary
                    document.getElementById("finalgrandtotal").innerText = formatPrice(grandTotal);
                    document.getElementById("roundoff").innerText = formatPrice(roundoff);
                    document.getElementById("payableamt").innerText = formatPrice(rounded);
                },
                error: function (xhr, status, error) {
                    console.error("Failed to load billing table:", error);
                }
            });
        }


        document.addEventListener('DOMContentLoaded', function () {
            const detailTable = document.getElementById("detailTable");
            if (detailTable) detailTable.style.display = "none";

            document.querySelectorAll('.pricing-block .inner-box').forEach(box => {
                box.addEventListener('click', () => {
                    // Toggle active plan
                    document.querySelectorAll('.pricing-block .inner-box').forEach(b => b.classList.remove('active'));
                    box.classList.add('active');

                    // Reload billing for selected plan
                    loadDeviceAndBillingTables();

                    // Show and scroll to detail table
                    if (detailTable) {
                        detailTable.style.display = "block";
                        detailTable.scrollIntoView({ behavior: 'smooth' });
                    }
                });
            });

            // Initial load
            loadSubscriptionData(true);
            loadDeviceAndBillingTables();
            window.handleProceedClick = function () {
                const activeBox = document.querySelector(".pricing-block .inner-box.active");

                if (!activeBox) {
                    alert("Please select a subscription plan before proceeding.");
                    return false;
                }

                document.getElementById("emiFrame").src = "EmiInstallment.aspx";
                $('#emiModal').modal('show');
                return false;
            };


        });



    </script>

</asp:Content>
