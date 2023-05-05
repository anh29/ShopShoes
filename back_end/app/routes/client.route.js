module.exports = function (app) {
    let clientController = require('../controllers/client.controller');
    app.get('/client', clientController.getList);
    
    // app.get('/client/:id', clientController.getById);    
    
    //nhận dữ liệu từ client gửi lên thông qua phương thức post (add)
    app.post('/client', clientController.addNew);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức put (update)
    // app.put('/customer', productController.update);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức delete
    // app.delete('/customer/:id', productController.delete);
};