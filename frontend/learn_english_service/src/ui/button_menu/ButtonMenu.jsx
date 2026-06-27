import { MenuIcon, X } from 'lucide-react';
import styles from '../button_menu/button_menu.module.css';

function ButtonMenu({isOpen, onToggle}){
    return(
        <>
            <div className={styles.button} onClick={onToggle}>
                {isOpen ? 
                    <X size={22} color='#b1b1b1'/> :
                    <MenuIcon size={22} color='#b1b1b1'/>} 
            </div>
        </>
    )
}
export default ButtonMenu;