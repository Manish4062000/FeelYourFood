<%@ Page Title="" Language="C#" MasterPageFile="~/FYF.Master" AutoEventWireup="true" CodeBehind="UploadDocuments.aspx.cs" Inherits="FeelYourFood.UploadDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-group span {
    font-weight: 700;
}
        .user-form p {
    text-align: right;
}
       hr{
               margin-bottom: 10px !important;
               margin-top: 10px !important;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
           <div class="col-md-2"></div>
            <div class="col-md-8">
                <div class="rest-details-sub">
                    <div class="user-details">

                        <div class="title">Bank Details </div>
                        <div class="user-form">
                            <div class="row">
                                <div class="col-md-12" align="center">
                                   Please make a one-time payment of <strong><span id="dueamounttopay">₹ <%= dueamounttopay %></span></strong> to the bank account mentioned below
                                </div>
                                <br /> <br /> 
                                <div class="clearfix"></div>                         

                                   
                                        <div class="col-md-6">
                                            <p>Bank Name</p>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="bankname" runat="server" Text="" CssClass="form-control-static"></asp:Label>
                                            </div>
                                        </div>
                                    
                                  
                                        <div class="col-md-6">
                                            <p>Branch</p>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="branch" runat="server" Text="" CssClass="form-control-static"></asp:Label>
                                            </div>
                                        </div>
                                   
                                   
                                        <div class="col-md-6">
                                            <p>Account Holder Name</p>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="holdername" runat="server" Text="" CssClass="form-control-static"></asp:Label>
                                            </div>
                                        </div>
                                   
                                        <div class="col-md-6">
                                            <p>Account Number</p>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="accountno" runat="server" Text="" CssClass="form-control-static"></asp:Label>
                                            </div>
                                        </div>
                                   
                                  
                                        <div class="col-md-6">
                                            <p>IFSC Code</p>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="ifsc" runat="server" Text="" CssClass="form-control-static"></asp:Label>
                                            </div>
                                        
                                    </div>
                                </div>
                            </div>
                        <hr />
                            <div class="row">
                                <div class="col-md-12" align="center">
                                    <p>Provide transaction ID and upload proof of payment.</p>
                                </div>
                                
                                        <div class="col-md-4" align="right">
                                            <label>Transaction/UTR Number</label>
                                        </div>
                                        <div class="col-md-6" >
                                            <div class="form-group">
                                                <asp:TextBox ID="transaction" runat="server" placeholder="Enter Transaction Number" required="" class="form-control"></asp:TextBox>
                                            </div>
                                            </div>
                                        <div class="col-md-4" align="right">
                                            <label>Upload File</label>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:FileUpload ID="myFile" runat="server" class="form-control" />
                                         
                                    </div>
                                </div>
                             
                            </div>
                        </div>
                
                    <div align="center">
                        <asp:Button ID="btnSubmit" runat="server" class="theme-btn" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
              <div class="col-md-2"></div>
        </div>
    </div>

    <script>
        function clearForm() {
            document.querySelector('.needs-validation')?.reset();

            const imagePreview = document.getElementById('<%= myFile.ClientID %>');
            if (imagePreview) {
                imagePreview.src = "#"; // or "", depending on your fallback
                imagePreview.style.display = "none";
            }
        }
    </script>
</asp:Content>
