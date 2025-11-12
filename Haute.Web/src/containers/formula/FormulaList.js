import React from 'react';
import { Button, Card, Col, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import FormulaThumbListView from 'containers/pages/FormulaThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const FormulaListing = ({
  items,
  allFormulaItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
}) => {
  return (
    <Row>
      <Colxx xxs="12">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Product Code
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Dispense Amount
              </p>
              <p className="w-20 list-item-heading mb-1 truncate font-weight-bold">
                Color Codes
              </p>
              <p className="w-20 list-item-heading mb-1 truncate font-weight-bold">
                Amounts
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((product) => {
        if (displayMode === 'thumblist') {
          return (
            <FormulaThumbListView
              key={product.ID}
              formula={product}
              isSelect={selectedItems.includes(product.ID)}
              collect={collect}
              onCheckItem={onCheckItem}
            />
          );
        }
        return <></>;
      })}
      {allFormulaItems === null ||
        (items.length === 0 && (
          <h3 className="mb-3 w-100 text-center">No Data Found</h3>
        ))}
      <Pagination
        currentPage={currentPage}
        totalPage={totalPage}
        onChangePage={(i) => onChangePage(i)}
      />
      <ContextMenuContainer
        onContextMenuClick={onContextMenuClick}
        onContextMenu={onContextMenu}
      />
    </Row>
  );
};

export default React.memo(FormulaListing);
