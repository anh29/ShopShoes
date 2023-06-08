module.exports = function (app) {
    let customerController = require('../controllers/customer.controller');
    app.get('/customer', customerController.getList);
    
    app.get('/customer/:id', customerController.getById);    
    
    //nhận dữ liệu từ client gửi lên thông qua phương thức post (add)
    app.post('/customer', customerController.addNew);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức put (update)
    // app.put('/customer', productController.update);
    
    // //nhận dữ liệu từ client gửi lên thông qua phương thức delete
    // app.delete('/customer/:id', productController.delete);
};