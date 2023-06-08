let bill = require('../models/bill.model');
var model = new bill();

exports.addNew = function(req, res) {
    console.log(req.body);
    model.create(req.body, function(err, data) {
        res.send({result: data, error: err});
    });
};