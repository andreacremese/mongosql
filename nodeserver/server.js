var express = require('express');
var app = express();
var MongoClient = require('mongodb').MongoClient;
var connectionString = process.env.mongoConectionString || require('./connectionString.js');
// the connection string is in a local file OR set in Azure in Application Settings as "mongoConectionString"

app.get('/api/mongo/reportsbypartnername/:partnerName', function (req, res) {

	var partnerName = req.params.partnerName;

	MongoClient.connect(connectionString, function(err, db) {
		if (err) {
			console.log(err);
			res.status(400).send(err);
			return;
		}
		var collection = db.collection("reports");
		collection.aggregate(
		[
			{$lookup : { from : "engineers", localField : "EngineerIds", foreignField : "_id", as : "engineer_docs"}},
			{$lookup : { from : "partners", localField : "PartnerId", foreignField : "_id", as : "partner_doc"}},
			{$match : {"partner_doc.Name" : partnerName}},
		]).toArray(function (err, docs) {
			res.send(docs);
		})
	});
})

app.get('/api/mongo/reportsbyengineername/:engName', function (req, res) {

	var engName = req.params.engName;

	MongoClient.connect(connectionString, function(err, db) {
		if (err) {
			console.log(err);
			res.status(400).send(err);
			return;
		}
		var collection = db.collection("reports");
		collection.aggregate(
		[
			{$lookup : { from : "engineers", localField : "EngineerIds", foreignField : "_id", as : "engineer_docs"}},
			{$lookup : { from : "partners", localField : "PartnerId", foreignField : "_id", as : "partner_doc"}},
			{$match : {"engineer_docs.Name" : engName}},
		]).toArray(function (err, docs) {
			res.send(docs);
		})
	});
})

app.get('/api/test', function (req, res) {
	res.status(200).send("OK");
} )
 

var port = process.env.PORT || 3000;
app.listen(port);