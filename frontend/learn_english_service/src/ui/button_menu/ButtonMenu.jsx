import { MenuIcon, X } from 'lucide-react';
import styles from '../button_menu/button_menu.module.css';

function ButtonMenu({isOpen, onToggle}){
    return(
        <>
            <div className={styles.button} onClick={onToggle}>
                {isOpen ? 
                    <X size={22} color='#6a6a6a'/> :
                    <MenuIcon size={22} color='#6a6a6a'/>} 
            </div>
        </>
    )
}
export default ButtonMenu;