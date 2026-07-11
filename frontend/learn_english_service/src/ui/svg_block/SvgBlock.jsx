import styles from '../svg_block/svg_block.module.css';

function SvgBlock({children, type}){
    return(
        <div className={`${styles.svg_block} ${styles[type]}`}>
            {children}
        </div>
    )
}

export default SvgBlock;