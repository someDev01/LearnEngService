import { MenuIcon, X } from 'lucide-react';
import styles from '../button_menu/button_menu.module.css';

function ButtonMenu({onOpen}){
    return(
        <>
            <div className={styles.button} onClick={onOpen}>
                <MenuIcon size={22} color='#b1b1b1'/>
            </div>
        </>
    )
}
export default ButtonMenu;