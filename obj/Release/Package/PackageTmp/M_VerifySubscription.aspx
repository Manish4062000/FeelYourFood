<%@ Page Title="" Language="C#" MasterPageFile="~/MFYF.Master" AutoEventWireup="true" CodeBehind="M_VerifySubscription.aspx.cs" Inherits="FeelYourFood.M_VerifySubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">

            <div class="col-md-12">
                <div class="rest-details-sub">
                    <div style="font-weight: bold">Verify Subscription</div>
                    <hr style="margin-top: 10px;" />
                    <div class="bg-wrapper table-responsive">
                        <asp:GridView ID="gvverify" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No Record Found" DataKeyNames="RestId" CssClass="table table-bordered table-striped table-responsive-md"
                            HeaderStyle-CssClass="thead-dark" OnPageIndexChanging="gvverify_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ResturantName" HeaderText="Resturant Name" />
                                <asp:BoundField DataField="RestAddress" HeaderText="Rest Address" />
                                <asp:BoundField DataField="Name" HeaderText="Owner Name" />
                                <asp:BoundField DataField="MobileNo" HeaderText=" Mobile No." />

                                <asp:TemplateField HeaderText="Apply Date">
                                    <ItemTemplate>
                                        <%# Eval("Date", "{0:dd/MM/yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransactionId" HeaderText="Transaction Id" />
                                <asp:BoundField DataField="payableamount" HeaderText="Payment Amount" />
                                <asp:TemplateField HeaderText="View File">
                                    <ItemTemplate>
                                        <a href='<%# ResolveUrl(Eval("UploadedFile").ToString()) %>'
                                            target="_blank"
                                            class="btn btn-sm btn-primary">View
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# Convert.ToBoolean(Eval("ActiveStatus")) ? "text-success" : "text-danger" %>'>
                                            <i class="fa fa-circle"></i><%# Convert.ToBoolean(Eval("ActiveStatus")) ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Verify">
                                    <ItemTemplate>
                                        <asp:Button
                                            ID="btnverify"
                                            runat="server"
                                            Text='<%# Convert.ToBoolean(Eval("ActiveStatus")) ? "Verified" : "Verify" %>'
                                            CssClass="btn btn-sm btn-primary confirm-btn"
                                            CommandArgument='<%# Eval("RestId") %>'
                                            CommandName='<%# Convert.ToBoolean(Eval("ActiveStatus")) ? "deactivate" : "activate" %>'
                                            OnCommand="btnverify_Command"
                                            UseSubmitBehavior="false"
                                            OnClientClick="return false;" />
                                        <input type="hidden" class="action-type" value='<%# Convert.ToBoolean(Eval("ActiveStatus")) ? "deactivate" : "activate" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            const buttons = document.querySelectorAll('.confirm-btn');

            buttons.forEach(function (btn) {
                btn.addEventListener('click', function (e) {
                    e.preventDefault();

                    const action = btn.parentElement.querySelector('.action-type').value;
                    let msg = (action === "deactivate")
                        ? "Are you sure you want to deactivate the plan?"
                        : "Are you sure you want to activate the plan?";

                    console.log("Action:", action, "Button Name:", btn.name);

                    if (confirm(msg)) {
                        setTimeout(function () {
                            __doPostBack(btn.name, '');
                        }, 100); // slight delay to ensure stability
                    }
                });
            });
        });
    </script>



</asp:Content>
