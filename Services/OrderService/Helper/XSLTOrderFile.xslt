<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <html>
      <body>
        <xsl:apply-templates/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="CompanyInfo">
    <h3>Company Information</h3>
    <span>
      Company Title:
      <xsl:value-of select="MemberTitle"/>
    </span>
    <br />
    <span>
      CVR:
      <xsl:value-of select="CVRNumber"/>
    </span>
    <br />

    <xsl:if test="DIMemberID !=''">
      <span>
        Member Id:
        <xsl:value-of select="DIMemberID"/>
      </span>
      <br />
    </xsl:if>
    <xsl:if test="Q8AccountNumber !=''">
      <span>
        Q8 Account Number:
        <xsl:value-of select="Q8AccountNumber"/>
      </span>
      <br />
    </xsl:if>

    <xsl:if test="SupplierCustomerNumber !=''">
      <span>
        Supplier Customer Number:
        <xsl:value-of select="SupplierCustomerNumber"/>
      </span>
      <br />
    </xsl:if>

    <span>
      P Number:
      <xsl:value-of select="PNumber"/>
    </span>
    <br />

    <xsl:if test="Account !=''">
      <span>
        Account:
        <xsl:value-of select="Account"/>
      </span>
      <br />
    </xsl:if>

    <xsl:if test="PartnerId !=''">
      <span>
        Partner Id:
        <xsl:value-of select="PartnerId"/>
      </span>
      <br />
    </xsl:if>

    <span>
      Contact Person:
      <xsl:value-of select="MemberName"/>
    </span>
    <br />

    <span>
      Address:
      <xsl:value-of select="InvoiceAddress"/>
    </span>
    <br />

    <span>
      Email:
      <xsl:value-of select="MemberUserInternalEmail"/>
    </span>
    <br />

    <span>
      Phone:
      <xsl:value-of select="MemberPhone"/>
    </span>
    <br />
  </xsl:template>

  <xsl:template match="OrderDetail">
    <h3>Order Detail</h3>
    <table id="tbl" border="1">
      <tr>
        <th>Key</th>
        <th>Value</th>
      </tr>
      <xsl:for-each select="Field">
        <tr>
          <td>
            <xsl:value-of select="Key" />
          </td>
          <td>
            <xsl:value-of select="Value" />
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>

  <!--<xsl:template match="key">

    <xsl:value-of select="."/>

  </xsl:template>

  <xsl:template match="Value">

    <xsl:value-of select="."/>

  </xsl:template>-->

</xsl:stylesheet>
