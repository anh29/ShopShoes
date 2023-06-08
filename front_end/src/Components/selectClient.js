sessionStorage.setItem('Employee', 'Duc Van');
window.addEventListener('onload', function() {  
    const client= document.querySelector('.input-box-client');
    client.text = sessionStorage.getItem('Employee');
});    
const selectClient = () => { 
    const text = sessionStorage.getItem("Employee");
    return (            
        <div className="find-client">
            <input type="text" className="input-box-client" style={{paddingLeft: '40px'}} value={text} readOnly/>
      </div>
    );
} 
export default selectClient