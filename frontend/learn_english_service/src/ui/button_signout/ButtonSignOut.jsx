import { LogOutIcon } from 'lucide-react';
import styles from '../button_signout/button_signout.module.css';

function ButtonSignOut({onClick}){
    return(
        <div className={styles.button_signout} onClick={onClick}>
            <LogOutIcon size={20} color='#cd0e00'/>
            <p>Выйти</p>
        </div>
    )
}

export default ButtonSignOut;