import styles from '../logo/logo.module.css';

function Logo({title}){
    return(
        <div className={styles.logo}>
            <p className={styles.title}>{title}</p>
        </div>
    )
}

export default Logo;