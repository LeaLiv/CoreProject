

const uri='https://localhost:7116/Users'
const checkDetails=async (event)=>{
    event.preventDefault();
    const username=document.getElementById('username').value;
    const password=document.getElementById('password').value;
    const loginuser={
        "UserName": username,
        "Password": password
    }
    const response= await fetch(uri,{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginuser)
    })

    console.log(loginuser);
    if(response.ok){
        const token=await response.json();
        localStorage.setItem("token",token);
        
        window.location.href='show.html'
    }
    
    
}


