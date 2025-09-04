<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="NewSubscription.aspx.cs" Inherits="FeelYourFood.NewSubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


    <style>
        /* Date:05:07:2025 */

        .sub-emi .panel-heading {
            background: #276b79;
            color: #fff;
        }



        .accordion-header {
            background: #e3ecef;
            padding: 12px 16px;
            margin-bottom: 2px;
            cursor: pointer;
            font-weight: bold;
            position: relative;
            border: 1px solid #ddd;
        }

            .accordion-header .glyphicon {
                position: absolute;
                right: 20px;
                top: 50%;
                transform: translateY(-50%);
                transition: transform 0.3s;
            }

            .accordion-header.collapsed .glyphicon {
                transform: translateY(-50%) rotate(0deg);
            }

            .accordion-header:not(.collapsed) .glyphicon {
                transform: translateY(-50%) rotate(180deg);
            }

        .accordion-body {
            border: 1px solid #ddd;
            border-top: none;
            padding: 15px;
            display: none;
        }

            .accordion-body label i {
                color: #1bb646;
                font-size: 16px;
            }

            .accordion-body label {
                display: flow;
                font-weight: 400;
                font-size: 15px;
            }

        .theme-btn-pay {
            padding: 13px 25px 13px 25px;
            background: #276b79;
            border: 1px solid transparent;
            box-shadow: 0px 4px 18px rgba(0, 0, 0, 0.1);
            color: #fff;
            text-transform: uppercase;
            font-weight: 700;
            font-size: 14px;
            letter-spacing: 1.5px;
            border-radius: 5px;
        }

        .calculateemi {
            padding: 5px 10px !important;
        }

        .sub-table-2 table th,
        .sub-table-2 table td,
        .sub-table table th,
        .sub-table table td {
            text-align: center;
            vertical-align: middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Subscription</h3>

    </div>
    <div class="wrapper">
        <div class="row">
            <asp:HiddenField ID="hfRestId" runat="server" />
            <div class="col-md-12">
                <div class="panel">

                    <div class="form-horizontal sub-table">
                        <table class="table table-bordered text-center">
                            <thead>
                                <tr>
                                    <th>Sl.No</th>
                                    <th>Device</th>
                                    <th>Device Price (Per piece)</th>
                                    <th>Quantity</th>
                                    <th>Total Device price</th>
                                    <th>Subscription rate</th>
                                </tr>
                            </thead>

                            <tbody></tbody>

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
                            <tbody></tbody>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-md-9"></div>
                        <div class="col-md-3">
                            <div class="price-list">

                                <label>Total Amount: <span id="finalgrandtotal"></span></label>
                                <label>Roundoff: <span id="roundoff"></span></label>
                                <label>Payable Amount: <span id="payableamt"></span></label>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-md-12">
                <div class="panel">

                    <div class="accordion-group" id="customAccordion">

                        <div class="sub-emi">
                            <div class="accordion-header collapsed" data-target="#item1">
                                <input type="radio" id="option1" name="basic" class="payment-radio" value="item1" style="margin-right: 10px;">
                                <label for="option1" style="margin-bottom: 0; cursor: pointer;">One Time Payment</label>
                                <span class="glyphicon glyphicon-chevron-down"></span>
                            </div>
                            <div id="item1" class="accordion-body">
                                <label><i class="fa fa-check-square-o" aria-hidden="true"></i>1 Year Free Subscription</label>

                                <p style="margin-top: 10px;">
                                    Enjoy a full year of access to all premium features at no cost. 
                                    No hidden charges, no commitment — just activate and explore.
                                </p>
                                <div align="center">
                                    <asp:Button ID="btnProceed1" runat="server" CssClass="theme-btn" Text="Proceed" OnClick="btnProceed1_Click" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="sub-emi">
                            <div class="accordion-header collapsed" data-target="#item2">
                                <input type="radio" id="option2" name="basic" class="payment-radio" value="item2" style="margin-right: 10px;">
                                <label for="option2" style="margin-bottom: 0; cursor: pointer;">EMI <span><em>(Equated Monthly Installment)</em></span></label>
                                <span class="glyphicon glyphicon-chevron-down"></span>
                            </div>
                            <div id="item2" class="accordion-body collapse">
                                <div class="down-pay">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label>Down Payment</label>
                                        </div>
                                        <div class="col-md-3">

                                            <input name="downpayment" value="" placeholder="Enter Down Payment" type="number" class="form-control">
                                        </div>
                                        <div class="col-md-2 ">
                                            <button type="button" class="theme-btn calculateemi" style="">Calculate</button>
                                        </div>
                                        <div class="col-md-5"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-4">
                                            <p><i style="font-size: 12px;">(Down payment must be <b>₹20,000</b> or higher.)</i></p>
                                        </div>

                                    </div>
                                    <br />
                                    <section class="pricing-section" id="pricingSection">

                                        <div class="outer-box">

                                            <div class="row">
                                                <!-- Pricing Block -->
                                                <div class="pricing-block col-lg-4 col-md-6 col-sm-12 wow fadeInUp">
                                                    <div class="inner-box" id="planQuarterly" data-multiplier="1" data-emitype="Quarterly">
                                                        <div class="icon-box">
                                                            <div class="icon-outer"><i class="fa fa-location-arrow"></i></div>
                                                        </div>
                                                        <div class="price-box">
                                                            <div class="title">Quarterly </div>
                                                            <h4 class="price"><span id="totalQuarterly">0</span></h4>
                                                        </div>
                                                        <ul class="features">
                                                            <li class="true">Due Amount <span id="dueAmountQuarterly">0</span></li>
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
                                                            <li class="true">Due Amount <span id="dueAmountHalfYearly">0</span></li>
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
                                                            <li class="true">Due Amount <span id="dueAmountYearly">0</span></li>
                                                        </ul>
                                                        <div class="btn-box">
                                                            <a href="#" class="theme-btn">BUY plan</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </section>
                                    <div align="center" id="proceedButtonSection">
                                        <asp:Button ID="Button" runat="server" CssClass="theme-btn" Text="Proceed" OnClientClick="return handleProceedClick();" />
                                    </div>
                                </div>
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

    <!-- Bootstrap Modal -->

    <!--body wrapper end-->
    <script>
        function scrollToElementById(id) {
            const el = document.getElementById(id);
            if (el) {
                el.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        }
        document.addEventListener("DOMContentLoaded", function () {

            // === Utility Functions ===
            function formatPrice(value) {
                return new Intl.NumberFormat('en-IN', {
                    style: 'currency',
                    currency: 'INR',
                    minimumFractionDigits: 2
                }).format(value || 0);
            }

            function parseAmount(text) {
                return parseFloat((text || "").replace(/[₹,]/g, '')) || 0;
            }

            function validateBeforeSubmit() {
                const activeCard = document.querySelector(".pricing-block .inner-box.active");
                if (!activeCard) {
                    alert("Please select an EMI plan before proceeding.");
                    return false;
                }
                return true;
            }

            // === Device and Subscription Table Load ===
            function loadDeviceSubscriptionTable() {
                const restId = document.getElementById("<%= hfRestId.ClientID %>").value;

                $.ajax({
                    type: "POST",
                    url: "NewSubscription.aspx/showdevicedata",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ restid: restId }),
                    dataType: "json",
                    success: function (response) {
                        const data = response.d || [];
                        const tbody = document.querySelector(".sub-table table tbody");
                        const subTable2Body = document.querySelector(".sub-table-2 table tbody");

                        tbody.innerHTML = "";
                        subTable2Body.innerHTML = "";

                        let totalDevicePrice = 0, totalSubscriptionPrice = 0;
                        let deviceDiscount = 0, subscriptionDiscount = 0;
                        let deviceGSTPercent = 0, softwareGSTPercent = 0;

                        data.forEach((item, index) => {
                            if (index === 0) {
                                deviceDiscount = parseFloat(item.DeviceDiscount) || 0;
                                subscriptionDiscount = parseFloat(item.SubscriptionDiscount) || 0;
                                deviceGSTPercent = parseFloat(item.DeviceGST) || 0;
                                softwareGSTPercent = parseFloat(item.SoftwareGST) || 0;
                            }

                            totalDevicePrice += parseFloat(item.DevicePrice);
                            totalSubscriptionPrice += parseFloat(item.SubscriptionPrice);

                            const row = `
                        <tr>
                            <td>${index + 1}</td>
                            <td>${item.DeviceName}</td>
                            <td>${formatPrice(item.DevPrice)}</td>
                            <td>${item.Quantity}</td>
                            <td>${formatPrice(item.DevicePrice)}</td>
                            <td>${formatPrice(item.SubscriptionPrice)}</td>
                        </tr>`;
                            tbody.insertAdjacentHTML("beforeend", row);
                        });

                        // === DEVICE Calculation ===
                        const deviceDiscountAmount = (totalDevicePrice * deviceDiscount) / 100;
                        const deviceTaxable = totalDevicePrice - deviceDiscountAmount;
                        const deviceGST = (deviceTaxable * deviceGSTPercent) / 100;
                        const deviceTotal = deviceTaxable + deviceGST;

                        // === SUBSCRIPTION Calculation ===
                        const subscriptionDiscountAmount = (totalSubscriptionPrice * subscriptionDiscount) / 100;
                        const subscriptionTaxable = totalSubscriptionPrice - subscriptionDiscountAmount;
                        const subscriptionGST = (subscriptionTaxable * softwareGSTPercent) / 100;
                        const subscriptionTotal = subscriptionTaxable + subscriptionGST;

                        // === Insert Final Table Rows ===
                        const deviceRow = `
                    <tr>
                        <td>1</td>
                        <td>Device Charges</td>
                        <td>${formatPrice(totalDevicePrice)}</td>
                        <td>${deviceDiscount}%</td>
                        <td>${formatPrice(deviceTaxable)}</td>
                        <td>${deviceGSTPercent}%</td>
                        <td>${formatPrice(deviceGST)}</td>
                        <td>${formatPrice(deviceTotal)}</td>
                    </tr>`;
                        subTable2Body.insertAdjacentHTML("beforeend", deviceRow);

                        const subscriptionRow = `
                    <tr>
                        <td>2</td>
                        <td>Subscription Charges</td>
                        <td>${formatPrice(totalSubscriptionPrice)}</td>
                        <td>${subscriptionDiscount}%</td>
                        <td>${formatPrice(subscriptionTaxable)}</td>
                        <td>${softwareGSTPercent}%</td>
                        <td>${formatPrice(subscriptionGST)}</td>
                        <td>${formatPrice(subscriptionTotal)}</td>
                    </tr>`;
                        subTable2Body.insertAdjacentHTML("beforeend", subscriptionRow);

                        const grandTotal = deviceTotal + subscriptionTotal;
                        const roundedPayable = Math.round(grandTotal);
                        const roundOff = roundedPayable - grandTotal;

                        document.getElementById("finalgrandtotal").innerText = formatPrice(grandTotal);
                        document.getElementById("roundoff").innerText = formatPrice(roundOff);
                        document.getElementById("payableamt").innerText = formatPrice(roundedPayable);

                        $.ajax({
                            type: "POST",
                            url: "NewSubscription.aspx/StorePayableAmount",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ amount: roundedPayable }),
                            success: () => console.log("Payable amount stored in session."),
                            error: (xhr, status, error) => console.error("Failed to store payable amount:", error)
                        });
                    },
                    error: (xhr, status, error) => console.error("Failed to load data:", error)
                });
            }

            loadDeviceSubscriptionTable();

            // === EMI Card Click Highlight ===
            document.querySelectorAll('.pricing-block .inner-box').forEach(box => {
                box.addEventListener('click', () => {
                    document.querySelectorAll('.pricing-block .inner-box').forEach(b => b.classList.remove('active'));
                    box.classList.add('active');

                    // SCROLL TO PROCEED BUTTON
                    scrollToElementById("proceedButtonSection");
                });
            });


            // === Accordion Header Click to Select Radio ===
            document.querySelectorAll('.accordion-header').forEach(header => {
                header.addEventListener('click', function () {
                    const radio = this.querySelector('input[type="radio"]');
                    if (radio && !radio.checked) {
                        radio.checked = true;
                        radio.dispatchEvent(new Event('change'));
                    }
                });
            });

            // === Handle Payment Plan Expansion ===
            document.querySelectorAll('.payment-radio').forEach(radio => {
                radio.addEventListener('change', function () {
                    document.querySelectorAll('.accordion-body').forEach(body => body.style.display = 'none');
                    document.querySelectorAll('.accordion-header').forEach(header => header.classList.add('collapsed'));

                    const activeBody = document.getElementById(this.value);
                    const activeHeader = this.closest('.accordion-header');
                    if (activeBody && activeHeader) {
                        activeBody.style.display = 'block';
                        activeHeader.classList.remove('collapsed');
                    }
                });
            });

            // === Trigger Default EMI Selection ===
            const defaultChecked = document.querySelector('.payment-radio:checked');
            if (defaultChecked) {
                defaultChecked.dispatchEvent(new Event('change'));
            }

            // === Calculate EMI ===
            document.querySelector('.calculateemi')?.addEventListener('click', function (e) {
                e.preventDefault();

                const downPaymentInput = document.querySelector('input[name="downpayment"]');
                const downPayment = parseAmount(downPaymentInput.value);
                const payableAmt = parseAmount(document.getElementById("payableamt").innerText);

                if (isNaN(downPayment) || downPayment < 20000) {
                    alert("Please enter a valid Down Payment of ₹20,000 or more.");
                    downPaymentInput.focus();
                    return;
                }

                const remaining = payableAmt - downPayment;
                if (remaining < 0) {
                    alert("Down Payment cannot be greater than Payable Amount.");
                    return;
                }

                const plans = [
                    { id: "Quarterly", multiplier: 4 },
                    { id: "HalfYearly", multiplier: 2 },
                    { id: "Yearly", multiplier: 1 }
                ];

                plans.forEach(plan => {
                    const emi = remaining / plan.multiplier;
                    document.getElementById(`total${plan.id}`).innerText = formatPrice(emi);
                    document.getElementById(`dueAmount${plan.id}`).innerText = formatPrice(remaining);
                });

                scrollToElementById("pricingSection");
            });

            // === Proceed Button Logic ===
            window.handleProceedClick = function () {
                const downPaymentInput = document.querySelector('input[name="downpayment"]');
                const downPayment = parseFloat((downPaymentInput.value || "").replace(/[₹,]/g, '')) || 0;
                const selectedPlan = getSelectedPlan();

                if (isNaN(downPayment) || downPayment < 20000) {
                    alert("Please enter a valid Down Payment of ₹20,000 or more.");
                    downPaymentInput.focus();
                    return false;
                }

                if (!selectedPlan) {
                    alert("Please select an EMI plan.");
                    return false;
                }
                if (!validateBeforeSubmit()) {
                    return false;
                }

                const activeCard = document.querySelector(".pricing-block .inner-box.active");
                let priceValue = "0";
                if (activeCard) {
                    const priceSpan = activeCard.querySelector(".price-box .price span");
                    priceValue = priceSpan ? priceSpan.innerText.trim().replace(/[₹,]/g, '') : "0";
                }

                const emiAmount = parseFloat(priceValue) || 0;

                console.log("Sending EMI Amount to server:", emiAmount);

                $.ajax({
                    type: "POST",
                    url: "NewSubscription.aspx/StoreEMIData",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        downPayment: downPayment,
                        selectedPlan: selectedPlan,
                        emiAmount: emiAmount // 👈 Pass to backend
                    }),
                    success: function () {
                        document.getElementById("emiFrame").src = "EmiInstallment.aspx";
                        $('#emiModal').modal('show');
                    },
                    error: function () {
                        alert("Something went wrong while storing data.");
                    }
                });

                return false;
            };

            function getSelectedPlan() {
                const activeCard = document.querySelector(".pricing-block .inner-box.active");
                if (activeCard) {
                    return activeCard.getAttribute("data-emitype") || "";
                }
                return "";
            }



        });
    </script>




</asp:Content>

