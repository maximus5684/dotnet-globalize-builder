DotNet Globalize Builder
=======

This is an ASP.NET handler written in C#. The purpose of this handler is to allow you to use [jquery/globalize](https://github.com/jquery/globalize) with minimal headache from the CLDR data.

Requirements
------------

You must download the following in order to use this handler:

* [rxaviers/cldrjs](https://github.com/rxaviers/cldrjs)
* The CLDR data (see the ReadMe for Globalize below).
* [jquery/globalize](https://github.com/jquery/globalize)

Installation and Usage
----------------------

I recommend extracting the necessary files from the requirements above to a temporary location, then copying the required files to your ~/Scripts folder. However, you can change the default folder by modifying two lines in the handler. All of the information about which json and .js files are necessary from the CLDR data and Globalize (respectively) can be found in the [jquery/globalize](https://github.com/jquery/globalize) README.

You will need to include a pretty big list of files to make Globalize work but some of them are optional. See the [jquery/globalize](https://github.com/jquery/globalize) README for details about which files are required for which functions. *Prior to any of these, you should include jQuery*. jQuery is included by default in new ASP.NET WebForms projects in Visual Studio 2013 when targeting .NET 4.5+. Here is an example of how you could include the required scripts for Globalize in the ScriptManager in ASP.NET 4.5+:

	<asp:ScriptManager runat="server">
		<Scripts>
			<%-- Default set of scripts, including jQuery, go here. --%>
			<asp:ScriptReference Path="~/Scripts/cldr.js" />
			<asp:ScriptReference Path="~/Scripts/cldr/event.js" />
			<asp:ScriptReference Path="~/Scripts/cldr/supplemental.js" />
			<asp:ScriptReference Path="~/Scripts/globalize.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/number.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/date.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/currency.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/message.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/plural.js" />
			<asp:ScriptReference Path="~/Scripts/globalize/relative-time.js" />
			<asp:ScriptReference Path="~/Scripts/GlobalizeBuilder.ashx" />
		</Scripts>
	</ScriptManager>
	
If you're targeting an older version of .NET or don't use the ScriptManager, here is a group of script tags that will do the same:

	<head runat="server">
		<!-- Make sure to include jQuery before this list. -->
		<script type="text/javascript" src="~/Scripts/cldr.js"></script>
		<script type="text/javascript" src="~/Scripts/cldr/event.js"></script>
		<script type="text/javascript" src="~/Scripts/cldr/supplemental.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/number.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/date.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/currency.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/message.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/plural.js"></script>
		<script type="text/javascript" src="~/Scripts/globalize/relative-time.js"></script>
		<script type="text/javascript" src="~/Scripts/GlobalizeBuilder.ashx"></script>
	</head>
	
When all features are implemented and all CLDR data are used, the output of the handler weighs in at around 385KB. Some client-side caching settings are included to help with bandwidth and page load times but refreshing the page will re-download the contents of the script. Server-side caching of the output of the script would likely also help.