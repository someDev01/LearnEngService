import { CircleX } from 'lucide-react';
import styles from '../error/error_text.module.css';

function ErrorText({children}){
    return(
        <div className={styles.error_button}>
            <p className={styles.error_text}>{children}</p>
        </div>
    )
}

export default ErrorText;