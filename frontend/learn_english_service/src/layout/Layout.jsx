import { Outlet } from "react-router-dom";
import Header from "../widgets/header/Header";
import Footer from "../widgets/footer/Footer";

function Layout(){
    return(
        <>
            <Header/>
            <Outlet/>
            <Footer/>
        </>
    )
}

export default Layout;