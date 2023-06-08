let Student = require('../models/customer.model');
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

// exports.getById = function(req, res) {
//     model.getOne(req.params.id, function(err, data) {
//         res.send({result: data, error: err});
//     })
// };

exports.addNew = function(req, res) {
    model.create(req.body, function(err, data) {
        res.send({result: data, error: err});
    });
};

exports.updateCore = async function(req, res) {
    model.udpateCore(req.body, function(err, data) {
        res.send({result: data, error: err});
    });
};
// exports.delete =  async function(req, res) {
//     model.delete(req.params.id, function(err, data) {
//         res.send({result: data, error: err});
//     });
// };