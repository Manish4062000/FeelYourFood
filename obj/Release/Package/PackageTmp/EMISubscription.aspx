<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="EMISubscription.aspx.cs" Inherits="FeelYourFood.EMISubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table {
            margin-bottom: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-heading">
        <h3>Payment Methods</h3>
        <ul class="breadcrumb">
            <li>
                <a href="#">Dashboard</a>
            </li>
            <li class="active">Payment Methods </li>
        </ul>

    </div>
    <!--body wrapper start-->
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="emi-page">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="order">
                                <h3>EMI Installment Plan</h3>
                                <span>
                                    <asp:Literal ID="lblSubscriptionType" runat="server" /></span>
                                <%--<h5>Last Payment:
                                    <asp:Literal ID="lblLastPayment" runat="server" /></h5>--%>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="order-num">
                                <span>Due Amount</span>
                                <h3>₹
                                    <asp:Literal ID="lblDueAmount" runat="server" /></h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <div class="emi-page">
                    <div class="paid-em">
                        <img src="images/Coins.gif" class="img-responsive" alt="" />
                        <h5>Total Amount</h5>
                        <h3>
                            <asp:Literal ID="lblTotalAmount" runat="server" /></h3>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="emi-page">
                    <div class="paid-em">
                        <div class="row">
                            <div class="col-md-6">
                                <h5>Paid Amount</h5>
                                <h3>
                                    <asp:Literal ID="lblPaidAmount" runat="server" /></h3>
                            </div>
                            <div class="col-md-6" style="text-align: end; color: #089f1e; font-weight: 600; font-size: 15px;">
                                <h6 style="font-size: 13px; font-weight: 600; color: #898989;">Last Payment Date</h6>
                                <asp:Literal ID="lblLastPayment" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="emi-page">
                    <div class="paid-em">
                        <div class="row">
                            <div class="col-md-6">
                                <h5>Next Installment</h5>
                                <h3>
                                    <asp:Literal ID="lblNextInstallment" runat="server" /></h3>
                            </div>

                            <div class="col-md-6" style="text-align: end; color: #ee490f; font-weight: 600; font-size: 15px;">
                                <h6 style="font-size: 13px; font-weight: 600; color: #898989;">Last Payment Date</h6>
                                <asp:Literal ID="lblNextPayment" runat="server" />
                                <asp:HiddenField ID="hfNextInstallment" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12" align="center">
                <asp:Button ID="btnShowEmiModal" runat="server" CssClass="theme-btn" Text="Proceed" OnClientClick="return showEmiPopup();" />

            </div>
        </div>

        <div class="panel" style="margin-top: 40px;">

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
                                    <table class="table table-bordered text-center" id="emiDetailTable">
                                        <thead>
                                            <tr>
                                                <th>Sl No</th>
                                                <th>Total Amount</th>
                                                <th>Subscription Type</th>
                                                <th>EMI Amount</th>
                                                <th>Paid Amount</th>
                                                <th>Remaining Amount</th>
                                                <th>Last Payment</th>
                                                <th>Next Payment</th>
                                            </tr>
                                        </thead>
                                        <tbody runat="server" id="emiTableBody"></tbody>
                                    </table>
                                </div>

                            </div>

                        </div>

                    </div>
                </div>


            </div>
        </div>

    </div>
    <div class="modal fade" id="emiModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document" style="max-width: 700px;">
            <div class="modal-content">
                <div class="modal-body" style="padding: 0px;">
                    <iframe id="emiFrame" src="" width="100%" height="500px" style="border: none;"></iframe>
                </div>
            </div>
        </div>
    </div>
    

    <script>
        function showEmiPopup() {
            const nextInstallment = document.getElementById('<%= hfNextInstallment.ClientID %>').value;

            if (!nextInstallment) {
                alert("Next Installment value not available.");
                return false;
            }

            // Store only the next installment in session
            $.ajax({
                type: "POST",
                url: "EMISubscription.aspx/StoreNextInstallment",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ nextInstallment: nextInstallment }),
                success: function () {
                    document.getElementById("emiFrame").src = "EmiInstallment.aspx";
                    $('#emiModal').modal('show');
                },
                error: function () {
                    alert("Failed to store next installment.");
                }
            });

            return false;
        }

    </script>
    <!--body wrapper end-->
</asp:Content>
