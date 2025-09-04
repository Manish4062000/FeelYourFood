<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmiInstallment.aspx.cs" Inherits="FeelYourFood.EmiInstallment" %>

<!DOCTYPE html>

<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="ThemeBucket">
    <link rel="shortcut icon" href="images/icon.png" type="image/png">

    <title>Feel Your Food. </title>

    <link href="css/style.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>


    <link href="css/style-responsive.css" rel="stylesheet">
    <style>
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="wrapper">
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-8" align="center">
                        <div class="rest-details-sub1">
                            <div class="user-details">
                                <div class="user-form">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="amu-pr">
                                                <h6>Total Amount to pay : <span id="DueAmounttopay" runat="server">₹0</span></h6>
                                                <h5>Scan & Pay to complete your Payment</h5>
                                                <div class="qrcodesize">
                                                    <div id="upiQrImage" style="display: none;"></div><br />
                                                </div>
                                                <div id="timerSection" style="display: none;">
                                                    <span id="timeLeftDisplay"></span>
                                                </div>

                                                <div align="center" style="margin-top: 10px;">
                                                    <asp:Button ID="btnSubmit" runat="server" CssClass="theme-btn" Text="Cancel" OnClientClick="return handleButtonClick();" />
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>

                    <div class="col-md-2"></div>
                </div>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">
    let seconds = 60;
    let qrDuration = 60;
    let timer;
    let pollInterval;

    let upiSettings = null;
    let qrCodeInstance = null;
    let trnId = null; // Dynamically populated from QR response

    const timeSpan = document.getElementById("timeLeftDisplay");
    const btnSubmit = document.getElementById("<%= btnSubmit.ClientID %>");
    const qrImage = document.getElementById("upiQrImage");
    const timerSection = document.getElementById("timerSection");

    function startTimer() {
        clearInterval(timer);
        updateTimer();
        timer = setInterval(updateTimer, 1000);
    }

    function updateTimer() {
        if (seconds <= 0) {
            clearInterval(timer);
            clearInterval(pollInterval);
            timeSpan.innerText = "Time Left: 0 sec";
            btnSubmit.value = "Retry";
            timerSection.style.display = "none";
            qrImage.style.display = "none";
            return;
        }
        timeSpan.innerText = `Time Left: ${seconds} sec`;
        seconds--;
    }

    function handleButtonClick() {
        if (btnSubmit.value === "Retry") {
            seconds = qrDuration;
            btnSubmit.value = "Cancel";
            qrImage.innerHTML = "";
            qrImage.style.display = "block";
            timerSection.style.display = "block";
            fetchUpiSettings(); // Re-fetch settings and regenerate QR
            return false;
        }

        // Close modal or cancel
        if (window.parent && typeof window.parent.$ === "function") {
            window.parent.$('#emiModal').modal('hide');
        }
        return false;
    }

    window.handleButtonClick = handleButtonClick;

    function fetchUpiSettings() {
        fetch('EmiInstallment.aspx/GetUpiSettings', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({})
        })
            .then(res => res.json())
            .then(data => {
                upiSettings = data.d;
                console.log("UPI Settings:", upiSettings);
                qrDuration = parseInt(upiSettings.QRDuration) || 60;
                seconds = qrDuration;
                generateQr();
            })
            .catch(err => {
                console.error("Error fetching UPI settings:", err);
                qrImage.innerHTML = "<p style='color:red;'>Failed to load UPI settings</p>";
            });
    }

    function generateQr() {
        if (!upiSettings?.RP_UserName || !upiSettings?.RP_AppKey || !upiSettings?.BaseUrlGenerate) {
            console.error("Missing UPI settings.");
            qrImage.innerHTML = "<p style='color:red;'>UPI Config error</p>";
            return;
        }

        fetch('EmiInstallment.aspx/GenerateUpiQr', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({})
        })
            .then(res => res.json())
            .then(data => {
                const qrUrl = data.d;
                console.log("QR URL from server:", qrUrl);

                if (!qrUrl) {
                    console.error("QR URL is null");
                    qrImage.innerHTML = "<p style='color:red;'>Failed to generate QR Code</p>";
                    timerSection.style.display = "none";
                    return;
                }

                // Extract txnId from the QR URL (tr=... param)
                const urlParams = new URLSearchParams(qrUrl.split('?')[1]);
                trnId = urlParams.get("tr");
                console.log("Extracted TrnId:", trnId);

                if (qrUrl.startsWith("upi://")) {
                    qrImage.innerHTML = "";
                    qrCodeInstance = new QRCode(qrImage, {
                        text: qrUrl,
                        width: 200,
                        height: 200,
                        colorDark: "#000000",
                        colorLight: "#ffffff",
                        correctLevel: QRCode.CorrectLevel.H
                    });
                    qrImage.style.display = "block";
                    timerSection.style.display = "block";
                    startTimer();
                    startPolling();
                } else {
                    qrImage.innerHTML = "<p style='color:red;'>Invalid QR URL</p>";
                    timerSection.style.display = "none";
                }
            })

            .catch(err => {
                console.error("Error generating QR:", err);
                qrImage.innerHTML = "<p style='color:red;'>QR generation failed</p>";
                timerSection.style.display = "none";
            });
    }

    function startPolling() {
        if (!trnId) {
            console.error("No transaction ID found for polling.");
            return;
        }

        clearInterval(pollInterval);

        pollInterval = setInterval(() => {
            fetch('EmiInstallment.aspx/CheckPaymentStatus', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({})
            })
                .then(res => res.json())
                .then(data => {
                    const status = data.d;
                    console.log("Payment status:", status);

                    if (status === "PAID") {
                        clearInterval(pollInterval);
                        clearInterval(timer);
                        alert("✅ Payment Successful!\nThank you for your payment.");
                        // Close modal if using Bootstrap modal
                        if (window.parent && typeof window.parent.$ === "function") {
                            window.parent.$('#emiModal').modal('hide');
                        }

                        // Redirect parent window to Subscription.aspx
                        if (window.parent) {
                            window.parent.location.href = "Subscription.aspx";
                        }
                    }

                })
                .catch(err => console.error("Polling error:", err));
        }, 2000);
    }

    document.addEventListener("DOMContentLoaded", fetchUpiSettings);
</script>









