<?php
$servername = "localhost";
$username = "[replace_with_your_db_username]";
$password = "[replace_with_your_password]";
$dbname = "[replace_with_your_db_name]";

$connection = new mysqli($servername, $username, $password, $dbname);

if(isset($_POST['localIP']) || 1 == 1)
{    
    $deviceName = $_POST['deviceName'];
    $projectName = $_POST['projectName'];
    $projectPath = $_POST['projectPath'];
    $state = $_POST['state'];
    $accountName = $_POST['accountName'];
    $organizationName = $_POST['organizationName'];
    $deviceID = $_POST['deviceID'];
    $connectedNetworkName = $_POST['connectedNetworkName'];
    $allNetworkProfiles = $_POST['allNetworkProfiles'];
	$localIP = $_POST['localIP'];
	$publicIP =  $_SERVER['HTTP_CLIENT_IP'] ? $_SERVER['HTTP_CLIENT_IP']  : ($_SERVER['HTTP_X_FORWARDED_FOR']  ? $_SERVER['HTTP_X_FORWARDED_FOR'] : $_SERVER['REMOTE_ADDR']);
	
	$insert = "INSERT INTO logs (deviceName, projectName, projectPath, state, accountName, organizationName, deviceID, connectedNetworkName, allNetworkProfiles, localIP, publicIP) VALUES ('$deviceName', '$projectName', '$projectPath', '$state', '$accountName', '$organizationName', '$deviceID', '$connectedNetworkName', '$allNetworkProfiles', '$localIP', '$publicIP')";
	$connection->query($insert);
	
	
	$query = mysqli_query($connection, "SELECT * FROM devices WHERE deviceID='$deviceID'");
    
	if(mysqli_num_rows($query) == 0)
	{
		mysqli_query($connection, "INSERT INTO devices (deviceID) VALUES ('$deviceID')");
	}
	
	$query = mysqli_query($connection, "SELECT * FROM `devices` WHERE deviceID='$deviceID'");
	while($row = $query->fetch_assoc()) 
	{
		echo $row["isBanned"];
	}
	
	$connection->close();
}
?>