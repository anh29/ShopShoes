module.exports = function (app) {
    let billController = require('../controllers/detailBill.controller');
    app.post('/detailBill', billController.addNew);
};