import { LogIn } from 'lucide-react';
import styles from '../button_signin/button_signin.module.css';

function ButtonSignIn({onClick}){
    return(
        <div className={styles.button_signin} onClick={onClick}>
            <p>Начать изучение</p>
            <LogIn size={20} color='rgb(255, 119, 0)'/>
        </div>
    )
}

export default ButtonSignIn;