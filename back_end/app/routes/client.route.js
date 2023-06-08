module.exports = function (app) {
    let clientController = require('../controllers/client.controller');
    app.get('/client', clientController.getList);
};