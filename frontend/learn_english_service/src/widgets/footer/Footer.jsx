import { Mail } from 'lucide-react';
import styles from '../footer/footer.module.css';

function Footer(){
    return(
        <div className={styles.footer}>
            <div className={styles.content}>
                <div className={styles.desc_block}>
                    <p className={styles.logo}>VoClipy</p>
                    <p>© 2026 VoClipy</p>
                </div>
                <div className={styles.contact}>
                    <Mail size={16} color='white'/>
                    <p>test@gmail.com</p>
                </div>
            </div>
        </div>
        
    )
}

export default Footer;