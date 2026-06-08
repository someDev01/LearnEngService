import styles from '../progress_cart/progress_cart.module.css';

function ProgressCart({count, text, type}){
    return(
        <div className={styles.cart}>
            <p className={`${styles.count} ${styles[type]}`}>{count}</p>
            <div className={styles.progress_text}>{text}</div>
        </div>
    )
}

export default ProgressCart;