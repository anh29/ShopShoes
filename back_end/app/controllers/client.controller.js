let Student = require('../models/client.model');
var model = new Student();
exports.getList = function(req, res) {
    model.getAll(function(err, data) {
        if(!err) {
            res.send({result: data});
        }else {
            res.send({error: err});
        }
    }); 
};
